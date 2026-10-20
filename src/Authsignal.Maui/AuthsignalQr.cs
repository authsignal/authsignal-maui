namespace Authsignal.Maui;

/// <summary>
/// QR code verification with a device credential.
/// </summary>
public interface IAuthsignalQr
{
	/// <summary>
	/// Gets the QR code credential stored on this device. <see cref="AuthsignalResponse{T}.Data"/> is null when there is none.
	/// </summary>
	Task<AuthsignalResponse<AppCredential?>> GetCredentialAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a QR code credential on this device. <paramref name="requireUserAuthentication"/> and <paramref name="keychainAccess"/> apply on iOS only.
	/// </summary>
	Task<AuthsignalResponse<AppCredential>> AddCredentialAsync(
		string? token = null,
		bool requireUserAuthentication = false,
		KeychainAccess? keychainAccess = null,
		bool performAttestation = false,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Removes the QR code credential from this device.
	/// </summary>
	Task<AuthsignalResponse<bool>> RemoveCredentialAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Claims the challenge encoded in a scanned QR code.
	/// </summary>
	Task<AuthsignalResponse<ClaimChallengeResponse>> ClaimChallengeAsync(string challengeId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Approves or rejects a claimed QR code challenge.
	/// </summary>
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
