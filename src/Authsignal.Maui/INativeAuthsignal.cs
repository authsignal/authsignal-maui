namespace Authsignal.Maui;

interface INativeAuthsignal
{
	void SetToken(string token);

	bool PasskeyIsSupported();

	Task<NativeResult> GetDeviceIdAsync(CancellationToken cancellationToken);

	Task<NativeResult> PasskeySignUpAsync(string? token, string? username, string? displayName, bool ignorePasskeyAlreadyExistsError, bool syncCredentials, CancellationToken cancellationToken);

	Task<NativeResult> PasskeySignInAsync(string? action, string? token, bool autofill, bool preferImmediatelyAvailableCredentials, bool syncCredentials, CancellationToken cancellationToken);

	void PasskeyCancel();

	Task<NativeResult> PasskeyShouldPromptToCreatePasskeyAsync(string? username, CancellationToken cancellationToken);

	Task<NativeResult> EmailEnrollAsync(string email, CancellationToken cancellationToken);

	Task<NativeResult> EmailChallengeAsync(CancellationToken cancellationToken);

	Task<NativeResult> EmailVerifyAsync(string code, CancellationToken cancellationToken);

	Task<NativeResult> SmsEnrollAsync(string phoneNumber, CancellationToken cancellationToken);

	Task<NativeResult> SmsChallengeAsync(CancellationToken cancellationToken);

	Task<NativeResult> SmsVerifyAsync(string code, CancellationToken cancellationToken);

	Task<NativeResult> TotpEnrollAsync(CancellationToken cancellationToken);

	Task<NativeResult> TotpVerifyAsync(string code, CancellationToken cancellationToken);

	Task<NativeResult> WhatsAppChallengeAsync(CancellationToken cancellationToken);

	Task<NativeResult> WhatsAppVerifyAsync(string code, CancellationToken cancellationToken);

	Task<NativeResult> PushGetCredentialAsync(CancellationToken cancellationToken);

	Task<NativeResult> PushAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, bool performAttestation, string? pushToken, CancellationToken cancellationToken);

	Task<NativeResult> PushRemoveCredentialAsync(CancellationToken cancellationToken);

	Task<NativeResult> PushGetChallengeAsync(CancellationToken cancellationToken);

	Task<NativeResult> PushUpdateChallengeAsync(string challengeId, bool approved, string? verificationCode, CancellationToken cancellationToken);

	Task<NativeResult> PushUpdateCredentialAsync(string? pushToken, bool resetExpiry, CancellationToken cancellationToken);

	Task<NativeResult> QrGetCredentialAsync(CancellationToken cancellationToken);

	Task<NativeResult> QrAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, bool performAttestation, CancellationToken cancellationToken);

	Task<NativeResult> QrRemoveCredentialAsync(CancellationToken cancellationToken);

	Task<NativeResult> QrClaimChallengeAsync(string challengeId, CancellationToken cancellationToken);

	Task<NativeResult> QrUpdateChallengeAsync(string challengeId, bool approved, string? verificationCode, CancellationToken cancellationToken);

	Task<NativeResult> InAppGetCredentialAsync(string? username, CancellationToken cancellationToken);

	Task<NativeResult> InAppAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, string? username, bool performAttestation, CancellationToken cancellationToken);

	Task<NativeResult> InAppRemoveCredentialAsync(string? username, CancellationToken cancellationToken);

	Task<NativeResult> InAppVerifyAsync(string? action, string? username, CancellationToken cancellationToken);

	Task<NativeResult> InAppCreatePinAsync(string pin, string username, string? token, CancellationToken cancellationToken);

	Task<NativeResult> InAppVerifyPinAsync(string pin, string username, string? action, CancellationToken cancellationToken);

	Task<NativeResult> InAppDeletePinAsync(string username, CancellationToken cancellationToken);

	Task<NativeResult> InAppGetAllPinUsernamesAsync(CancellationToken cancellationToken);
}
