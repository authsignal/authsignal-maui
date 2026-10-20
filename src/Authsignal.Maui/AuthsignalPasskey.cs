namespace Authsignal.Maui;

public interface IAuthsignalPasskey
{
	bool IsSupported { get; }

	Task<AuthsignalResponse<SignUpResponse>> SignUpAsync(
		string? token = null,
		string? username = null,
		string? displayName = null,
		bool ignorePasskeyAlreadyExistsError = false,
		bool syncCredentials = false,
		CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<SignInResponse>> SignInAsync(
		string? action = null,
		string? token = null,
		bool preferImmediatelyAvailableCredentials = true,
		bool syncCredentials = false,
		CancellationToken cancellationToken = default);

	void Cancel();

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
