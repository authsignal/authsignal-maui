using Authsignal.Maui.Binding.iOS;

namespace Authsignal.Maui;

sealed class NativeAuthsignal(AuthsignalOptions options) : INativeAuthsignal
{
	readonly AuthsignalMaui native = new(options.TenantId, options.BaseUrl, options.DeviceId, AuthsignalClient.Version);

	public void SetToken(string token) => native.SetToken(token);

	public bool PasskeyIsSupported() => native.PasskeyIsSupported();

	public Task<NativeResult> GetDeviceIdAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.GetDeviceId(completion), cancellationToken);

	public Task<NativeResult> PasskeySignUpAsync(string? token, string? username, string? displayName, bool ignorePasskeyAlreadyExistsError, bool syncCredentials, CancellationToken cancellationToken) =>
		Invoke(completion => native.PasskeySignUp(token, username, displayName, ignorePasskeyAlreadyExistsError, syncCredentials, completion), cancellationToken);

	public Task<NativeResult> PasskeySignInAsync(string? action, string? token, bool preferImmediatelyAvailableCredentials, bool syncCredentials, CancellationToken cancellationToken) =>
		Invoke(completion => native.PasskeySignIn(action, token, preferImmediatelyAvailableCredentials, syncCredentials, completion), cancellationToken);

	public void PasskeyCancel() => native.PasskeyCancel();

	public Task<NativeResult> PasskeyShouldPromptToCreatePasskeyAsync(string? username, CancellationToken cancellationToken) =>
		Invoke(completion => native.PasskeyShouldPromptToCreatePasskey(username, completion), cancellationToken);

	public Task<NativeResult> EmailEnrollAsync(string email, CancellationToken cancellationToken) =>
		Invoke(completion => native.EmailEnroll(email, completion), cancellationToken);

	public Task<NativeResult> EmailChallengeAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.EmailChallenge(completion), cancellationToken);

	public Task<NativeResult> EmailVerifyAsync(string code, CancellationToken cancellationToken) =>
		Invoke(completion => native.EmailVerify(code, completion), cancellationToken);

	public Task<NativeResult> SmsEnrollAsync(string phoneNumber, CancellationToken cancellationToken) =>
		Invoke(completion => native.SmsEnroll(phoneNumber, completion), cancellationToken);

	public Task<NativeResult> SmsChallengeAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.SmsChallenge(completion), cancellationToken);

	public Task<NativeResult> SmsVerifyAsync(string code, CancellationToken cancellationToken) =>
		Invoke(completion => native.SmsVerify(code, completion), cancellationToken);

	public Task<NativeResult> TotpEnrollAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.TotpEnroll(completion), cancellationToken);

	public Task<NativeResult> TotpVerifyAsync(string code, CancellationToken cancellationToken) =>
		Invoke(completion => native.TotpVerify(code, completion), cancellationToken);

	public Task<NativeResult> WhatsAppChallengeAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.WhatsAppChallenge(completion), cancellationToken);

	public Task<NativeResult> WhatsAppVerifyAsync(string code, CancellationToken cancellationToken) =>
		Invoke(completion => native.WhatsAppVerify(code, completion), cancellationToken);

	public Task<NativeResult> PushGetCredentialAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.PushGetCredential(completion), cancellationToken);

	public Task<NativeResult> PushAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, bool performAttestation, string? pushToken, CancellationToken cancellationToken) =>
		Invoke(completion => native.PushAddCredential(token, requireUserAuthentication, keychainAccess, performAttestation, pushToken, completion), cancellationToken);

	public Task<NativeResult> PushRemoveCredentialAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.PushRemoveCredential(completion), cancellationToken);

	public Task<NativeResult> PushGetChallengeAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.PushGetChallenge(completion), cancellationToken);

	public Task<NativeResult> PushUpdateChallengeAsync(string challengeId, bool approved, string? verificationCode, CancellationToken cancellationToken) =>
		Invoke(completion => native.PushUpdateChallenge(challengeId, approved, verificationCode, completion), cancellationToken);

	public Task<NativeResult> PushUpdateCredentialAsync(string? pushToken, bool resetExpiry, CancellationToken cancellationToken) =>
		Invoke(completion => native.PushUpdateCredential(pushToken, resetExpiry, completion), cancellationToken);

	public Task<NativeResult> QrGetCredentialAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.QrGetCredential(completion), cancellationToken);

	public Task<NativeResult> QrAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, bool performAttestation, CancellationToken cancellationToken) =>
		Invoke(completion => native.QrAddCredential(token, requireUserAuthentication, keychainAccess, performAttestation, completion), cancellationToken);

	public Task<NativeResult> QrRemoveCredentialAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.QrRemoveCredential(completion), cancellationToken);

	public Task<NativeResult> QrClaimChallengeAsync(string challengeId, CancellationToken cancellationToken) =>
		Invoke(completion => native.QrClaimChallenge(challengeId, completion), cancellationToken);

	public Task<NativeResult> QrUpdateChallengeAsync(string challengeId, bool approved, string? verificationCode, CancellationToken cancellationToken) =>
		Invoke(completion => native.QrUpdateChallenge(challengeId, approved, verificationCode, completion), cancellationToken);

	public Task<NativeResult> InAppGetCredentialAsync(string? username, CancellationToken cancellationToken) =>
		Invoke(completion => native.InAppGetCredential(username, completion), cancellationToken);

	public Task<NativeResult> InAppAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, string? username, bool performAttestation, CancellationToken cancellationToken) =>
		Invoke(completion => native.InAppAddCredential(token, requireUserAuthentication, keychainAccess, username, performAttestation, completion), cancellationToken);

	public Task<NativeResult> InAppRemoveCredentialAsync(string? username, CancellationToken cancellationToken) =>
		Invoke(completion => native.InAppRemoveCredential(username, completion), cancellationToken);

	public Task<NativeResult> InAppVerifyAsync(string? action, string? username, CancellationToken cancellationToken) =>
		Invoke(completion => native.InAppVerify(action, username, completion), cancellationToken);

	public Task<NativeResult> InAppCreatePinAsync(string pin, string username, string? token, CancellationToken cancellationToken) =>
		Invoke(completion => native.InAppCreatePin(pin, username, token, completion), cancellationToken);

	public Task<NativeResult> InAppVerifyPinAsync(string pin, string username, string? action, CancellationToken cancellationToken) =>
		Invoke(completion => native.InAppVerifyPin(pin, username, action, completion), cancellationToken);

	public Task<NativeResult> InAppDeletePinAsync(string username, CancellationToken cancellationToken) =>
		Invoke(completion => native.InAppDeletePin(username, completion), cancellationToken);

	public Task<NativeResult> InAppGetAllPinUsernamesAsync(CancellationToken cancellationToken) =>
		Invoke(completion => native.InAppGetAllPinUsernames(completion), cancellationToken);

	static Task<NativeResult> Invoke(Action<AuthsignalMauiCompletion> call, CancellationToken cancellationToken) =>
		NativeResult.FromCallback(complete => call((data, errorCode, error) => complete(new NativeResult(data, errorCode, error))), cancellationToken);
}
