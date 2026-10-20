namespace Authsignal.Maui;

public interface IAuthsignalQr
{
	Task<AuthsignalResponse<AppCredential?>> GetCredentialAsync(CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<AppCredential>> AddCredentialAsync(
		string? token = null,
		bool requireUserAuthentication = false,
		KeychainAccess? keychainAccess = null,
		bool performAttestation = false,
		CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<bool>> RemoveCredentialAsync(CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<ClaimChallengeResponse>> ClaimChallengeAsync(string challengeId, CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<bool>> UpdateChallengeAsync(string challengeId, bool approved, string? verificationCode = null, CancellationToken cancellationToken = default);
}

sealed class AuthsignalQr(INativeAuthsignal native) : IAuthsignalQr
{
	public Task<AuthsignalResponse<AppCredential?>> GetCredentialAsync(CancellationToken cancellationToken = default) =>
		native.QrGetCredentialAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.AppCredential)!;

	public Task<AuthsignalResponse<AppCredential>> AddCredentialAsync(
		string? token = null,
		bool requireUserAuthentication = false,
		KeychainAccess? keychainAccess = null,
		bool performAttestation = false,
		CancellationToken cancellationToken = default) =>
		native.QrAddCredentialAsync(token, requireUserAuthentication, keychainAccess.ToNativeValue(), performAttestation, cancellationToken)
			.ToResponseAsync(AuthsignalJsonContext.Default.AppCredential);

	public Task<AuthsignalResponse<bool>> RemoveCredentialAsync(CancellationToken cancellationToken = default) =>
		native.QrRemoveCredentialAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.Boolean);

	public Task<AuthsignalResponse<ClaimChallengeResponse>> ClaimChallengeAsync(string challengeId, CancellationToken cancellationToken = default) =>
		native.QrClaimChallengeAsync(challengeId, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.ClaimChallengeResponse);

	public Task<AuthsignalResponse<bool>> UpdateChallengeAsync(string challengeId, bool approved, string? verificationCode = null, CancellationToken cancellationToken = default) =>
		native.QrUpdateChallengeAsync(challengeId, approved, verificationCode, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.Boolean);
}
