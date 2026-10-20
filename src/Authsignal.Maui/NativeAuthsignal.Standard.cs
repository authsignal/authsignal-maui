#if !ANDROID && !IOS
namespace Authsignal.Maui;

sealed class NativeAuthsignal : INativeAuthsignal
{
	public NativeAuthsignal(AuthsignalOptions options) => throw new PlatformNotSupportedException("Authsignal is only supported on iOS and Android.");

	public void SetToken(string token) => throw new PlatformNotSupportedException();

	public bool PasskeyIsSupported() => throw new PlatformNotSupportedException();

	public Task<NativeResult> GetDeviceIdAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> PasskeySignUpAsync(string? token, string? username, string? displayName, bool ignorePasskeyAlreadyExistsError, bool syncCredentials, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> PasskeySignInAsync(string? action, string? token, bool autofill, bool preferImmediatelyAvailableCredentials, bool syncCredentials, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public void PasskeyCancel() => throw new PlatformNotSupportedException();

	public Task<NativeResult> PasskeyShouldPromptToCreatePasskeyAsync(string? username, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> EmailEnrollAsync(string email, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> EmailChallengeAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> EmailVerifyAsync(string code, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> SmsEnrollAsync(string phoneNumber, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> SmsChallengeAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> SmsVerifyAsync(string code, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> TotpEnrollAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> TotpVerifyAsync(string code, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> WhatsAppChallengeAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> WhatsAppVerifyAsync(string code, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> PushGetCredentialAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> PushAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, bool performAttestation, string? pushToken, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> PushRemoveCredentialAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> PushGetChallengeAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> PushUpdateChallengeAsync(string challengeId, bool approved, string? verificationCode, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> PushUpdateCredentialAsync(string? pushToken, bool resetExpiry, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> QrGetCredentialAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> QrAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, bool performAttestation, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> QrRemoveCredentialAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> QrClaimChallengeAsync(string challengeId, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> QrUpdateChallengeAsync(string challengeId, bool approved, string? verificationCode, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> InAppGetCredentialAsync(string? username, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> InAppAddCredentialAsync(string? token, bool requireUserAuthentication, string? keychainAccess, string? username, bool performAttestation, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> InAppRemoveCredentialAsync(string? username, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> InAppVerifyAsync(string? action, string? username, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> InAppCreatePinAsync(string pin, string username, string? token, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> InAppVerifyPinAsync(string pin, string username, string? action, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> InAppDeletePinAsync(string username, CancellationToken cancellationToken) => throw new PlatformNotSupportedException();

	public Task<NativeResult> InAppGetAllPinUsernamesAsync(CancellationToken cancellationToken) => throw new PlatformNotSupportedException();
}
#endif
