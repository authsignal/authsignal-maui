using Microsoft.Extensions.DependencyInjection;

namespace Authsignal.Maui.Tests;

public class AuthsignalClientTests
{
	readonly FakeNative native = new();

	AuthsignalClient Client => new(native);

	[Fact]
	public async Task DecodesSignInResponse()
	{
		native.Result = new("{\"isVerified\":true,\"token\":\"t\",\"userId\":\"u\",\"userAuthenticatorId\":\"a\",\"username\":\"n\",\"displayName\":\"d\"}", null, null);

		var response = await Client.Passkey.SignInAsync(action: "signIn", cancellationToken: TestContext.Current.CancellationToken);

		Assert.Null(response.ErrorCode);
		Assert.True(response.Data!.IsVerified);
		Assert.Equal("t", response.Data.Token);
		Assert.Equal("u", response.Data.UserId);
		Assert.Equal("a", response.Data.UserAuthenticatorId);
		Assert.Equal("n", response.Data.Username);
		Assert.Equal("d", response.Data.DisplayName);
		Assert.Equal("passkeySignIn:signIn::True:False", Assert.Single(native.Calls));
	}

	[Fact]
	public async Task DecodesMissingOptionalFieldsAsNull()
	{
		native.Result = new("{\"isVerified\":false}", null, null);

		var response = await Client.Email.VerifyAsync("000000", TestContext.Current.CancellationToken);

		Assert.False(response.Data!.IsVerified);
		Assert.Null(response.Data.Token);
		Assert.Null(response.Data.FailureReason);
		Assert.Equal("emailVerify:000000", Assert.Single(native.Calls));
	}

	[Fact]
	public async Task PassesSignUpArguments()
	{
		native.Result = new("{\"token\":\"t\"}", null, null);

		var response = await Client.Passkey.SignUpAsync("token", "user", "User", true, true, TestContext.Current.CancellationToken);

		Assert.Equal("t", response.Data!.Token);
		Assert.Equal("passkeySignUp:token:user:User:True:True", Assert.Single(native.Calls));
	}

	[Fact]
	public async Task MapsNativeErrors()
	{
		native.Result = new(null, "invalid_token", "Token is invalid or expired.");

		var response = await Client.Email.ChallengeAsync(TestContext.Current.CancellationToken);

		Assert.Null(response.Data);
		Assert.Equal("invalid_token", response.ErrorCode);
		Assert.Equal("Token is invalid or expired.", response.Error);
	}

	[Fact]
	public async Task MapsMissingDataToUnexpectedError()
	{
		var response = await Client.Email.EnrollAsync("user@example.com", TestContext.Current.CancellationToken);

		Assert.Null(response.Data);
		Assert.Equal("unexpected_error", response.ErrorCode);
	}

	[Fact]
	public async Task MapsMalformedDataToUnexpectedError()
	{
		native.Result = new("{", null, null);

		var response = await Client.Email.ChallengeAsync(TestContext.Current.CancellationToken);

		Assert.Null(response.Data);
		Assert.Equal("unexpected_error", response.ErrorCode);
	}

	[Fact]
	public async Task ReturnsDeviceId()
	{
		native.Result = new("\"device-id\"", null, null);

		Assert.Equal("device-id", await Client.GetDeviceIdAsync(TestContext.Current.CancellationToken));
	}

	[Fact]
	public void PassesTokenAndSupport()
	{
		native.Supported = false;
		var client = Client;

		client.SetToken("token");

		Assert.Equal("token", native.Token);
		Assert.False(client.Passkey.IsSupported);
	}

	[Fact]
	public async Task CancelsWhileWaitingForNativeResult()
	{
		native.Pending = new TaskCompletionSource<NativeResult>();
		using var cancellation = new CancellationTokenSource();

		var task = Client.Email.VerifyAsync("123456", cancellation.Token);
		cancellation.Cancel();

		await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
	}

	[Fact]
	public async Task ThrowsWhenAlreadyCancelled()
	{
		await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Client.Email.ChallengeAsync(new CancellationToken(true)));
		Assert.Empty(native.Calls);
	}

	[Fact]
	public async Task IgnoresNativeResultAfterCancellation()
	{
		native.Pending = new TaskCompletionSource<NativeResult>();
		using var cancellation = new CancellationTokenSource();

		var task = Client.Email.ChallengeAsync(cancellation.Token);
		cancellation.Cancel();
		native.Pending.SetResult(new("{\"challengeId\":\"c\"}", null, null));

		await Assert.ThrowsAnyAsync<OperationCanceledException>(() => task);
	}

	[Fact]
	public async Task RoutesSmsCalls()
	{
		native.Result = new("{\"userAuthenticatorId\":\"a\"}", null, null);
		Assert.Equal("a", (await Client.Sms.EnrollAsync("+64211234567", TestContext.Current.CancellationToken)).Data!.UserAuthenticatorId);

		native.Result = new("{\"challengeId\":\"c\"}", null, null);
		Assert.Equal("c", (await Client.Sms.ChallengeAsync(TestContext.Current.CancellationToken)).Data!.ChallengeId);

		native.Result = new("{\"isVerified\":true,\"token\":\"t\"}", null, null);
		Assert.Equal("t", (await Client.Sms.VerifyAsync("012345", TestContext.Current.CancellationToken)).Data!.Token);

		Assert.Equal(["smsEnroll:+64211234567", "smsChallenge", "smsVerify:012345"], native.Calls);
	}

	[Fact]
	public async Task DecodesTotpEnrollment()
	{
		native.Result = new("{\"userAuthenticatorId\":\"a\",\"uri\":\"otpauth://totp/x\",\"secret\":\"S\"}", null, null);

		var response = await Client.Totp.EnrollAsync(TestContext.Current.CancellationToken);

		Assert.Equal("a", response.Data!.UserAuthenticatorId);
		Assert.Equal("otpauth://totp/x", response.Data.Uri);
		Assert.Equal("S", response.Data.Secret);

		native.Result = new("{\"isVerified\":false,\"failureReason\":\"CODE_INVALID_OR_EXPIRED\"}", null, null);
		Assert.Equal("CODE_INVALID_OR_EXPIRED", (await Client.Totp.VerifyAsync("000000", TestContext.Current.CancellationToken)).Data!.FailureReason);
		Assert.Equal(["totpEnroll", "totpVerify:000000"], native.Calls);
	}

	[Fact]
	public async Task RoutesWhatsAppCalls()
	{
		native.Result = new("{\"challengeId\":\"c\"}", null, null);
		Assert.Equal("c", (await Client.WhatsApp.ChallengeAsync(TestContext.Current.CancellationToken)).Data!.ChallengeId);

		native.Result = new(null, "invalid_token", "Token is invalid or expired.");
		Assert.Equal("invalid_token", (await Client.WhatsApp.VerifyAsync("123456", TestContext.Current.CancellationToken)).ErrorCode);

		Assert.Equal(["whatsappChallenge", "whatsappVerify:123456"], native.Calls);
	}

	[Theory]
	[InlineData("true", true)]
	[InlineData("false", false)]
	public async Task DecodesShouldPromptToCreatePasskey(string data, bool expected)
	{
		native.Result = new(data, null, null);

		var response = await Client.Passkey.ShouldPromptToCreatePasskeyAsync("user", TestContext.Current.CancellationToken);

		Assert.Equal(expected, response.Data);
		Assert.Equal("passkeyShouldPrompt:user", Assert.Single(native.Calls));
	}

	[Fact]
	public void CancelsPasskeyRequest()
	{
		Client.Passkey.Cancel();

		Assert.Equal(1, native.CancelCount);
	}

	[Fact]
	public async Task ReturnsNullCredentialWithoutError()
	{
		native.Result = new("null", null, null);

		var response = await Client.Push.GetCredentialAsync(TestContext.Current.CancellationToken);

		Assert.Null(response.Data);
		Assert.Null(response.ErrorCode);
	}

	[Fact]
	public async Task MapsCredentialNotFound()
	{
		native.Result = new(null, "credential_not_found", null);

		var response = await Client.InApp.GetCredentialAsync("user", TestContext.Current.CancellationToken);

		Assert.Null(response.Data);
		Assert.Equal("credential_not_found", response.ErrorCode);
		Assert.Equal("inappGetCredential:user", Assert.Single(native.Calls));
	}

	[Fact]
	public async Task DecodesAppCredential()
	{
		native.Result = new("{\"credentialId\":\"c\",\"createdAt\":\"2026-10-19T00:00:00Z\",\"userId\":\"u\",\"expiresAt\":\"2026-11-19T00:00:00Z\"}", null, null);

		var response = await Client.Push.AddCredentialAsync("token", true, KeychainAccess.AfterFirstUnlock, true, "push-token", TestContext.Current.CancellationToken);

		Assert.Equal("c", response.Data!.CredentialId);
		Assert.Equal("u", response.Data.UserId);
		Assert.Null(response.Data.LastAuthenticatedAt);
		Assert.Equal("2026-11-19T00:00:00Z", response.Data.ExpiresAt);
		Assert.Equal("pushAddCredential:token:True:afterFirstUnlock:True:push-token", Assert.Single(native.Calls));
	}

	[Fact]
	public async Task OmitsDefaultKeychainAccess()
	{
		native.Result = new("{\"credentialId\":\"c\",\"createdAt\":\"t\",\"userId\":\"u\"}", null, null);

		await Client.Qr.AddCredentialAsync(cancellationToken: TestContext.Current.CancellationToken);
		await Client.InApp.AddCredentialAsync(keychainAccess: KeychainAccess.WhenPasscodeSetThisDeviceOnly, username: "pin-user", cancellationToken: TestContext.Current.CancellationToken);

		Assert.Equal(["qrAddCredential::False::False", "inappAddCredential::False:whenPasscodeSetThisDeviceOnly:pin-user:False"], native.Calls);
	}

	[Fact]
	public async Task DecodesPushChallengeWithCustomData()
	{
		native.Result = new("{\"challengeId\":\"ch\",\"actionCode\":\"signIn\",\"ipAddress\":\"1.2.3.4\",\"expiresAt\":1792431350,\"custom\":{\"amount\":42,\"nested\":{\"a\":true}},\"user\":{\"custom\":{\"tier\":\"gold\"}}}", null, null);

		var response = await Client.Push.GetChallengeAsync(TestContext.Current.CancellationToken);

		Assert.Equal("ch", response.Data!.ChallengeId);
		Assert.Equal(1792431350, response.Data.ExpiresAt);
		Assert.Equal(42, response.Data.Custom!["amount"].GetInt32());
		Assert.True(response.Data.Custom["nested"].GetProperty("a").GetBoolean());
		Assert.Equal("gold", response.Data.User!.Custom!["tier"].GetString());
	}

	[Fact]
	public async Task RoutesChallengeUpdates()
	{
		native.Result = new("true", null, null);

		Assert.True((await Client.Push.UpdateChallengeAsync("ch", true, "12", TestContext.Current.CancellationToken)).Data);
		Assert.True((await Client.Qr.UpdateChallengeAsync("qr", false, cancellationToken: TestContext.Current.CancellationToken)).Data);

		Assert.Equal(["pushUpdateChallenge:ch:True:12", "qrUpdateChallenge:qr:False:"], native.Calls);
	}

	[Fact]
	public async Task DecodesUpdatedPushCredential()
	{
		native.Result = new("{\"userAuthenticatorId\":\"a\",\"userId\":\"u\",\"lastVerifiedAt\":\"t\",\"pushToken\":\"p\"}", null, null);

		var response = await Client.Push.UpdateCredentialAsync("p", true, TestContext.Current.CancellationToken);

		Assert.Equal("p", response.Data!.PushToken);
		Assert.Null(response.Data.ExpiresAt);
		Assert.Equal("pushUpdateCredential:p:True", Assert.Single(native.Calls));
	}

	[Fact]
	public async Task DecodesQrClaim()
	{
		native.Result = new("{\"success\":true,\"userAgent\":\"ua\",\"custom\":{\"k\":\"v\"}}", null, null);

		var response = await Client.Qr.ClaimChallengeAsync("qr", TestContext.Current.CancellationToken);

		Assert.True(response.Data!.Success);
		Assert.Equal("v", response.Data.Custom!["k"].GetString());
		Assert.Null(response.Data.User);
	}

	[Fact]
	public async Task DecodesInAppAndPinResponses()
	{
		native.Result = new("{\"token\":\"t\",\"userId\":\"u\",\"userAuthenticatorId\":\"a\"}", null, null);
		var verify = await Client.InApp.VerifyAsync("signIn", "user", TestContext.Current.CancellationToken);

		native.Result = new("{\"isVerified\":false}", null, null);
		var pin = await Client.InApp.VerifyPinAsync("1234", "user", cancellationToken: TestContext.Current.CancellationToken);

		native.Result = new("[\"alice\",\"bob\"]", null, null);
		var usernames = await Client.InApp.GetAllPinUsernamesAsync(TestContext.Current.CancellationToken);

		Assert.Equal("t", verify.Data!.Token);
		Assert.Null(verify.Data.Username);
		Assert.False(pin.Data!.IsVerified);
		Assert.Equal(["alice", "bob"], usernames.Data!);
		Assert.Equal(["inappVerify:signIn:user", "inappVerifyPin:1234:user:", "inappGetAllPinUsernames"], native.Calls);
	}

	[Fact]
	public async Task MapsNullBooleanToUnexpectedError()
	{
		native.Result = new("null", null, null);

		var response = await Client.InApp.DeletePinAsync("user", TestContext.Current.CancellationToken);

		Assert.Equal("unexpected_error", response.ErrorCode);
	}

	[Theory]
	[InlineData("")]
	[InlineData(" ")]
	public void RejectsMissingTenantId(string tenantId)
	{
		Assert.Throws<ArgumentException>(() => new AuthsignalClient(tenantId));
	}

	[Fact]
	public void RejectsMissingBaseUrl()
	{
		Assert.Throws<ArgumentException>(() => new AuthsignalClient(new AuthsignalOptions { TenantId = "tenant", BaseUrl = "" }));
	}

	[Fact]
	public void ThrowsPlatformNotSupportedOutsideiOSAndAndroid()
	{
		Assert.Throws<PlatformNotSupportedException>(() => new AuthsignalClient("tenant"));
	}

	[Fact]
	public void RegistersClientWithMauiAppBuilder()
	{
		var builder = MauiApp.CreateBuilder();

		builder.UseAuthsignal(options => options.TenantId = "tenant");

		var descriptor = Assert.Single(builder.Services, s => s.ServiceType == typeof(IAuthsignalClient));
		Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
	}

	[Fact]
	public void UsesDefaultBaseUrl()
	{
		Assert.Equal("https://api.authsignal.com/v1", new AuthsignalOptions().BaseUrl);
	}

	[Fact]
	public void ReadsVersionFromAssembly()
	{
		Assert.Equal("0.1.0", AuthsignalClient.Version);
	}
}
