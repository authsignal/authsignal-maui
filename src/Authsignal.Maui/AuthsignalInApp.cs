namespace Authsignal.Maui;

/// <summary>
/// In-app and PIN verification with a device credential.
/// </summary>
public interface IAuthsignalInApp
{
	/// <summary>
	/// Gets the in-app credential stored on this device. <see cref="AuthsignalResponse{T}.Data"/> is null when there is none.
	/// </summary>
	Task<AuthsignalResponse<AppCredential?>> GetCredentialAsync(string? username = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates an in-app credential on this device. <paramref name="requireUserAuthentication"/> and <paramref name="keychainAccess"/> apply on iOS only.
	/// </summary>
	Task<AuthsignalResponse<AppCredential>> AddCredentialAsync(
		string? token = null,
		bool requireUserAuthentication = false,
		KeychainAccess? keychainAccess = null,
		string? username = null,
		bool performAttestation = false,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Removes the in-app credential from this device.
	/// </summary>
	Task<AuthsignalResponse<bool>> RemoveCredentialAsync(string? username = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Verifies the user with the in-app credential on this device.
	/// </summary>
	Task<AuthsignalResponse<InAppVerifyResponse>> VerifyAsync(string? action = null, string? username = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a PIN-protected in-app credential.
	/// </summary>
	Task<AuthsignalResponse<AppCredential>> CreatePinAsync(string pin, string username, string? token = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Verifies a PIN and, if correct, the in-app credential.
	/// </summary>
	Task<AuthsignalResponse<VerifyPinResponse>> VerifyPinAsync(string pin, string username, string? action = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a PIN and its in-app credential.
	/// </summary>
	Task<AuthsignalResponse<bool>> DeletePinAsync(string username, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets the usernames that have a PIN on this device.
	/// </summary>
	Task<AuthsignalResponse<string[]>> GetAllPinUsernamesAsync(CancellationToken cancellationToken = default);
}

sealed class AuthsignalInApp(INativeAuthsignal native) : IAuthsignalInApp
{
	public Task<AuthsignalResponse<AppCredential?>> GetCredentialAsync(string? username = null, CancellationToken cancellationToken = default) =>
		native.InAppGetCredentialAsync(username, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.AppCredential)!;

	public Task<AuthsignalResponse<AppCredential>> AddCredentialAsync(
		string? token = null,
		bool requireUserAuthentication = false,
		KeychainAccess? keychainAccess = null,
		string? username = null,
		bool performAttestation = false,
		CancellationToken cancellationToken = default) =>
		native.InAppAddCredentialAsync(token, requireUserAuthentication, keychainAccess.ToNativeValue(), username, performAttestation, cancellationToken)
			.ToResponseAsync(AuthsignalJsonContext.Default.AppCredential);

	public Task<AuthsignalResponse<bool>> RemoveCredentialAsync(string? username = null, CancellationToken cancellationToken = default) =>
		native.InAppRemoveCredentialAsync(username, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.Boolean);

	public Task<AuthsignalResponse<InAppVerifyResponse>> VerifyAsync(string? action = null, string? username = null, CancellationToken cancellationToken = default) =>
		native.InAppVerifyAsync(action, username, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.InAppVerifyResponse);

	public Task<AuthsignalResponse<AppCredential>> CreatePinAsync(string pin, string username, string? token = null, CancellationToken cancellationToken = default) =>
		native.InAppCreatePinAsync(pin, username, token, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.AppCredential);

	public Task<AuthsignalResponse<VerifyPinResponse>> VerifyPinAsync(string pin, string username, string? action = null, CancellationToken cancellationToken = default) =>
		native.InAppVerifyPinAsync(pin, username, action, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.VerifyPinResponse);

	public Task<AuthsignalResponse<bool>> DeletePinAsync(string username, CancellationToken cancellationToken = default) =>
		native.InAppDeletePinAsync(username, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.Boolean);

	public Task<AuthsignalResponse<string[]>> GetAllPinUsernamesAsync(CancellationToken cancellationToken = default) =>
		native.InAppGetAllPinUsernamesAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.StringArray);
}
