namespace Authsignal.Maui;

/// <summary>
/// Passkey sign-up and sign-in.
/// </summary>
public interface IAuthsignalPasskey
{
	/// <summary>
	/// Whether passkeys are supported on this device.
	/// </summary>
	bool IsSupported { get; }

	/// <summary>
	/// Creates a passkey for the user identified by the token.
	/// </summary>
	Task<AuthsignalResponse<SignUpResponse>> SignUpAsync(
		string? token = null,
		string? username = null,
		string? displayName = null,
		bool ignorePasskeyAlreadyExistsError = false,
		bool syncCredentials = false,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Signs in with a passkey, either for an action or with an existing token.
	/// </summary>
	Task<AuthsignalResponse<SignInResponse>> SignInAsync(
		string? action = null,
		string? token = null,
		bool preferImmediatelyAvailableCredentials = true,
		bool syncCredentials = false,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Cancels an in-progress passkey request. Has no effect on Android.
	/// </summary>
	void Cancel();

	/// <summary>
	/// Whether to prompt the user to create a passkey, because none is stored on this device for them.
	/// </summary>
	Task<AuthsignalResponse<bool>> ShouldPromptToCreatePasskeyAsync(string? username = null, CancellationToken cancellationToken = default);
}

sealed class AuthsignalPasskey(INativeAuthsignal native) : IAuthsignalPasskey
{
	public bool IsSupported => native.PasskeyIsSupported();

	public Task<AuthsignalResponse<SignUpResponse>> SignUpAsync(
		string? token = null,
		string? username = null,
		string? displayName = null,
		bool ignorePasskeyAlreadyExistsError = false,
		bool syncCredentials = false,
		CancellationToken cancellationToken = default) =>
		native.PasskeySignUpAsync(token, username, displayName, ignorePasskeyAlreadyExistsError, syncCredentials, cancellationToken)
			.ToResponseAsync(AuthsignalJsonContext.Default.SignUpResponse);

	public Task<AuthsignalResponse<SignInResponse>> SignInAsync(
		string? action = null,
		string? token = null,
		bool preferImmediatelyAvailableCredentials = true,
		bool syncCredentials = false,
		CancellationToken cancellationToken = default) =>
		native.PasskeySignInAsync(action, token, preferImmediatelyAvailableCredentials, syncCredentials, cancellationToken)
			.ToResponseAsync(AuthsignalJsonContext.Default.SignInResponse);

	public void Cancel() => native.PasskeyCancel();

	public Task<AuthsignalResponse<bool>> ShouldPromptToCreatePasskeyAsync(string? username = null, CancellationToken cancellationToken = default) =>
		native.PasskeyShouldPromptToCreatePasskeyAsync(username, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.Boolean);
}
