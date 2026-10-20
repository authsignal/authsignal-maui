namespace Authsignal.Maui.Tests;

sealed class FakeNative : INativeAuthsignal
{
	public NativeResult Result { get; set; } = new(null, null, null);

	public TaskCompletionSource<NativeResult>? Pending { get; set; }

	public List<string> Calls { get; } = [];

	public string? Token { get; private set; }

	public bool Supported { get; set; } = true;

	public void SetToken(string token) => Token = token;

	public bool PasskeyIsSupported() => Supported;

	public Task<NativeResult> GetDeviceIdAsync(CancellationToken cancellationToken) => Complete("getDeviceId", cancellationToken);

	public Task<NativeResult> PasskeySignUpAsync(string? token, string? username, string? displayName, bool ignorePasskeyAlreadyExistsError, bool syncCredentials, CancellationToken cancellationToken) =>
		Complete($"passkeySignUp:{token}:{username}:{displayName}:{ignorePasskeyAlreadyExistsError}:{syncCredentials}", cancellationToken);

	public Task<NativeResult> PasskeySignInAsync(string? action, string? token, bool preferImmediatelyAvailableCredentials, bool syncCredentials, CancellationToken cancellationToken) =>
		Complete($"passkeySignIn:{action}:{token}:{preferImmediatelyAvailableCredentials}:{syncCredentials}", cancellationToken);

	public int CancelCount { get; private set; }

	public void PasskeyCancel() => CancelCount++;

	public Task<NativeResult> PasskeyShouldPromptToCreatePasskeyAsync(string? username, CancellationToken cancellationToken) => Complete($"passkeyShouldPrompt:{username}", cancellationToken);

	public Task<NativeResult> EmailEnrollAsync(string email, CancellationToken cancellationToken) => Complete($"emailEnroll:{email}", cancellationToken);

	public Task<NativeResult> EmailChallengeAsync(CancellationToken cancellationToken) => Complete("emailChallenge", cancellationToken);

	public Task<NativeResult> EmailVerifyAsync(string code, CancellationToken cancellationToken) => Complete($"emailVerify:{code}", cancellationToken);

	public Task<NativeResult> SmsEnrollAsync(string phoneNumber, CancellationToken cancellationToken) => Complete($"smsEnroll:{phoneNumber}", cancellationToken);

	public Task<NativeResult> SmsChallengeAsync(CancellationToken cancellationToken) => Complete("smsChallenge", cancellationToken);

	public Task<NativeResult> SmsVerifyAsync(string code, CancellationToken cancellationToken) => Complete($"smsVerify:{code}", cancellationToken);

	public Task<NativeResult> TotpEnrollAsync(CancellationToken cancellationToken) => Complete("totpEnroll", cancellationToken);

	public Task<NativeResult> TotpVerifyAsync(string code, CancellationToken cancellationToken) => Complete($"totpVerify:{code}", cancellationToken);

	public Task<NativeResult> WhatsAppChallengeAsync(CancellationToken cancellationToken) => Complete("whatsappChallenge", cancellationToken);

	public Task<NativeResult> WhatsAppVerifyAsync(string code, CancellationToken cancellationToken) => Complete($"whatsappVerify:{code}", cancellationToken);

	public Task<NativeResult> PushGetCredentialAsync(CancellationToken cancellationToken) => Complete("pushGetCredential", cancellationToken);

	public Task<NativeResult> PushAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, bool performAttestation, string? pushToken, CancellationToken cancellationToken) => Complete($"pushAddCredential:{token}:{requireUserAuthentication}:{keychainAccess}:{performAttestation}:{pushToken}", cancellationToken);

	public Task<NativeResult> PushRemoveCredentialAsync(CancellationToken cancellationToken) => Complete("pushRemoveCredential", cancellationToken);

	public Task<NativeResult> PushGetChallengeAsync(CancellationToken cancellationToken) => Complete("pushGetChallenge", cancellationToken);

	public Task<NativeResult> PushUpdateChallengeAsync(string challengeId, bool approved, string? verificationCode, CancellationToken cancellationToken) => Complete($"pushUpdateChallenge:{challengeId}:{approved}:{verificationCode}", cancellationToken);

	public Task<NativeResult> PushUpdateCredentialAsync(string? pushToken, bool resetExpiry, CancellationToken cancellationToken) => Complete($"pushUpdateCredential:{pushToken}:{resetExpiry}", cancellationToken);

	public Task<NativeResult> QrGetCredentialAsync(CancellationToken cancellationToken) => Complete("qrGetCredential", cancellationToken);

	public Task<NativeResult> QrAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, bool performAttestation, CancellationToken cancellationToken) => Complete($"qrAddCredential:{token}:{requireUserAuthentication}:{keychainAccess}:{performAttestation}", cancellationToken);

	public Task<NativeResult> QrRemoveCredentialAsync(CancellationToken cancellationToken) => Complete("qrRemoveCredential", cancellationToken);

	public Task<NativeResult> QrClaimChallengeAsync(string challengeId, CancellationToken cancellationToken) => Complete($"qrClaimChallenge:{challengeId}", cancellationToken);

	public Task<NativeResult> QrUpdateChallengeAsync(string challengeId, bool approved, string? verificationCode, CancellationToken cancellationToken) => Complete($"qrUpdateChallenge:{challengeId}:{approved}:{verificationCode}", cancellationToken);

	public Task<NativeResult> InAppGetCredentialAsync(string? username, CancellationToken cancellationToken) => Complete($"inappGetCredential:{username}", cancellationToken);

	public Task<NativeResult> InAppAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, string? username, bool performAttestation, CancellationToken cancellationToken) => Complete($"inappAddCredential:{token}:{requireUserAuthentication}:{keychainAccess}:{username}:{performAttestation}", cancellationToken);

	public Task<NativeResult> InAppRemoveCredentialAsync(string? username, CancellationToken cancellationToken) => Complete($"inappRemoveCredential:{username}", cancellationToken);

	public Task<NativeResult> InAppVerifyAsync(string? action, string? username, CancellationToken cancellationToken) => Complete($"inappVerify:{action}:{username}", cancellationToken);

	public Task<NativeResult> InAppCreatePinAsync(string pin, string username, string? token, CancellationToken cancellationToken) => Complete($"inappCreatePin:{pin}:{username}:{token}", cancellationToken);

	public Task<NativeResult> InAppVerifyPinAsync(string pin, string username, string? action, CancellationToken cancellationToken) => Complete($"inappVerifyPin:{pin}:{username}:{action}", cancellationToken);

	public Task<NativeResult> InAppDeletePinAsync(string username, CancellationToken cancellationToken) => Complete($"inappDeletePin:{username}", cancellationToken);

	public Task<NativeResult> InAppGetAllPinUsernamesAsync(CancellationToken cancellationToken) => Complete("inappGetAllPinUsernames", cancellationToken);

	Task<NativeResult> Complete(string call, CancellationToken cancellationToken) =>
		NativeResult.FromCallback(complete =>
		{
			Calls.Add(call);

			if (Pending is { } pending)
			{
				pending.Task.ContinueWith(t => complete(t.Result), TaskScheduler.Default);
			}
			else
			{
				complete(Result);
			}
		}, cancellationToken);
}
