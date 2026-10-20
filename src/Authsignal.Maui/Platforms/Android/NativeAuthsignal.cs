using Com.Authsignal.Maui;

namespace Authsignal.Maui;

sealed class NativeAuthsignal(AuthsignalOptions options) : INativeAuthsignal
{
	readonly Lazy<AuthsignalMaui> native = new(() => new AuthsignalMaui(
		Platform.AppContext, options.TenantId, options.BaseUrl, options.DeviceId, AuthsignalClient.Version));

	public void SetToken(string token) => native.Value.SetToken(token);

	public bool PasskeyIsSupported() => native.Value.PasskeyIsSupported();

	public Task<NativeResult> GetDeviceIdAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.GetDeviceId(callback), cancellationToken);

	public Task<NativeResult> PasskeySignUpAsync(string? token, string? username, string? displayName, bool ignorePasskeyAlreadyExistsError, bool syncCredentials, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.PasskeySignUp(Platform.CurrentActivity, token, username, displayName, ignorePasskeyAlreadyExistsError, syncCredentials, callback), cancellationToken);

	public Task<NativeResult> PasskeySignInAsync(string? action, string? token, bool preferImmediatelyAvailableCredentials, bool syncCredentials, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.PasskeySignIn(Platform.CurrentActivity, action, token, preferImmediatelyAvailableCredentials, syncCredentials, callback), cancellationToken);

	public void PasskeyCancel() => native.Value.PasskeyCancel();

	public Task<NativeResult> PasskeyShouldPromptToCreatePasskeyAsync(string? username, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.PasskeyShouldPromptToCreatePasskey(username, callback), cancellationToken);

	public Task<NativeResult> EmailEnrollAsync(string email, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.EmailEnroll(email, callback), cancellationToken);

	public Task<NativeResult> EmailChallengeAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.EmailChallenge(callback), cancellationToken);

	public Task<NativeResult> EmailVerifyAsync(string code, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.EmailVerify(code, callback), cancellationToken);

	public Task<NativeResult> SmsEnrollAsync(string phoneNumber, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.SmsEnroll(phoneNumber, callback), cancellationToken);

	public Task<NativeResult> SmsChallengeAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.SmsChallenge(callback), cancellationToken);

	public Task<NativeResult> SmsVerifyAsync(string code, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.SmsVerify(code, callback), cancellationToken);

	public Task<NativeResult> TotpEnrollAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.TotpEnroll(callback), cancellationToken);

	public Task<NativeResult> TotpVerifyAsync(string code, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.TotpVerify(code, callback), cancellationToken);

	public Task<NativeResult> WhatsAppChallengeAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.WhatsappChallenge(callback), cancellationToken);

	public Task<NativeResult> WhatsAppVerifyAsync(string code, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.WhatsappVerify(code, callback), cancellationToken);

	public Task<NativeResult> PushGetCredentialAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.PushGetCredential(callback), cancellationToken);

	public Task<NativeResult> PushAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, bool performAttestation, string? pushToken, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.PushAddCredential(token, performAttestation, pushToken, callback), cancellationToken);

	public Task<NativeResult> PushRemoveCredentialAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.PushRemoveCredential(callback), cancellationToken);

	public Task<NativeResult> PushGetChallengeAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.PushGetChallenge(callback), cancellationToken);

	public Task<NativeResult> PushUpdateChallengeAsync(string challengeId, bool approved, string? verificationCode, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.PushUpdateChallenge(challengeId, approved, verificationCode, callback), cancellationToken);

	public Task<NativeResult> PushUpdateCredentialAsync(string? pushToken, bool resetExpiry, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.PushUpdateCredential(pushToken, resetExpiry, callback), cancellationToken);

	public Task<NativeResult> QrGetCredentialAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.QrGetCredential(callback), cancellationToken);

	public Task<NativeResult> QrAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, bool performAttestation, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.QrAddCredential(token, performAttestation, callback), cancellationToken);

	public Task<NativeResult> QrRemoveCredentialAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.QrRemoveCredential(callback), cancellationToken);

	public Task<NativeResult> QrClaimChallengeAsync(string challengeId, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.QrClaimChallenge(challengeId, callback), cancellationToken);

	public Task<NativeResult> QrUpdateChallengeAsync(string challengeId, bool approved, string? verificationCode, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.QrUpdateChallenge(challengeId, approved, verificationCode, callback), cancellationToken);

	public Task<NativeResult> InAppGetCredentialAsync(string? username, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.InappGetCredential(username, callback), cancellationToken);

	public Task<NativeResult> InAppAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, string? username, bool performAttestation, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.InappAddCredential(token, username, performAttestation, callback), cancellationToken);

	public Task<NativeResult> InAppRemoveCredentialAsync(string? username, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.InappRemoveCredential(username, callback), cancellationToken);

	public Task<NativeResult> InAppVerifyAsync(string? action, string? username, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.InappVerify(action, username, callback), cancellationToken);

	public Task<NativeResult> InAppCreatePinAsync(string pin, string username, string? token, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.InappCreatePin(pin, username, token, callback), cancellationToken);

	public Task<NativeResult> InAppVerifyPinAsync(string pin, string username, string? action, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.InappVerifyPin(pin, username, action, callback), cancellationToken);

	public Task<NativeResult> InAppDeletePinAsync(string username, CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.InappDeletePin(username, callback), cancellationToken);

	public Task<NativeResult> InAppGetAllPinUsernamesAsync(CancellationToken cancellationToken) =>
		Invoke(callback => native.Value.InappGetAllPinUsernames(callback), cancellationToken);

	static Task<NativeResult> Invoke(Action<IAuthsignalMauiCallback> call, CancellationToken cancellationToken) =>
		NativeResult.FromCallback(complete => call(new Callback(complete)), cancellationToken);

	sealed class Callback(Action<NativeResult> complete) : Java.Lang.Object, IAuthsignalMauiCallback
	{
		public void OnComplete(string? data, string? errorCode, string? error) =>
			complete(new NativeResult(data, errorCode, error));
	}
}
