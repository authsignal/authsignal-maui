namespace Authsignal.Maui;

public interface IAuthsignalPush
{
	Task<AuthsignalResponse<AppCredential?>> GetCredentialAsync(CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<AppCredential>> AddCredentialAsync(
		string? token = null,
		bool requireUserAuthentication = false,
		KeychainAccess? keychainAccess = null,
		bool performAttestation = false,
		string? pushToken = null,
		CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<bool>> RemoveCredentialAsync(CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<AppChallenge?>> GetChallengeAsync(CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<bool>> UpdateChallengeAsync(string challengeId, bool approved, string? verificationCode = null, CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<UpdatedAppCredential>> UpdateCredentialAsync(string? pushToken = null, bool resetExpiry = false, CancellationToken cancellationToken = default);
}

sealed class AuthsignalPush(INativeAuthsignal native) : IAuthsignalPush
{
	public Task<AuthsignalResponse<AppCredential?>> GetCredentialAsync(CancellationToken cancellationToken = default) =>
		native.PushGetCredentialAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.AppCredential)!;

	public Task<AuthsignalResponse<AppCredential>> AddCredentialAsync(
		string? token = null,
		bool requireUserAuthentication = false,
		KeychainAccess? keychainAccess = null,
		bool performAttestation = false,
		string? pushToken = null,
		CancellationToken cancellationToken = default) =>
		native.PushAddCredentialAsync(token, requireUserAuthentication, keychainAccess.ToNativeValue(), performAttestation, pushToken, cancellationToken)
			.ToResponseAsync(AuthsignalJsonContext.Default.AppCredential);

	public Task<AuthsignalResponse<bool>> RemoveCredentialAsync(CancellationToken cancellationToken = default) =>
		native.PushRemoveCredentialAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.Boolean);

	public Task<AuthsignalResponse<AppChallenge?>> GetChallengeAsync(CancellationToken cancellationToken = default) =>
		native.PushGetChallengeAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.AppChallenge)!;

	public Task<AuthsignalResponse<bool>> UpdateChallengeAsync(string challengeId, bool approved, string? verificationCode = null, CancellationToken cancellationToken = default) =>
		native.PushUpdateChallengeAsync(challengeId, approved, verificationCode, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.Boolean);

	public Task<AuthsignalResponse<UpdatedAppCredential>> UpdateCredentialAsync(string? pushToken = null, bool resetExpiry = false, CancellationToken cancellationToken = default) =>
		native.PushUpdateCredentialAsync(pushToken, resetExpiry, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.UpdatedAppCredential);
}
