namespace Authsignal.Maui;

/// <summary>
/// Push verification with a device credential.
/// </summary>
public interface IAuthsignalPush
{
	/// <summary>
	/// Gets the push credential stored on this device. <see cref="AuthsignalResponse{T}.Data"/> is null when there is none.
	/// </summary>
	Task<AuthsignalResponse<AppCredential?>> GetCredentialAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a push credential on this device. <paramref name="requireUserAuthentication"/> and <paramref name="keychainAccess"/> apply on iOS only.
	/// </summary>
	Task<AuthsignalResponse<AppCredential>> AddCredentialAsync(
		string? token = null,
		bool requireUserAuthentication = false,
		KeychainAccess? keychainAccess = null,
		bool performAttestation = false,
		string? pushToken = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Removes the push credential from this device.
	/// </summary>
	Task<AuthsignalResponse<bool>> RemoveCredentialAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets the pending push challenge for this device. <see cref="AuthsignalResponse{T}.Data"/> is null when there is none.
	/// </summary>
	Task<AuthsignalResponse<AppChallenge?>> GetChallengeAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Approves or rejects a push challenge.
	/// </summary>
	Task<AuthsignalResponse<bool>> UpdateChallengeAsync(string challengeId, bool approved, string? verificationCode = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates the push token or resets the expiry of the push credential.
	/// </summary>
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
