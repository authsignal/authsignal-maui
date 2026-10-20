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
		bool autofill = false,
		bool preferImmediatelyAvailableCredentials = true,
		bool syncCredentials = false,
		CancellationToken cancellationToken = default);

	void Cancel();

	Task<AuthsignalResponse<bool>> ShouldPromptToCreatePasskeyAsync(string? username = null, CancellationToken cancellationToken = default);
}

sealed class AuthsignalPasskey(INativeAuthsignal native) : IAuthsignalPasskey
{
	public bool IsSupported => native.PasskeyIsSupported();

	public async Task<AuthsignalResponse<SignUpResponse>> SignUpAsync(
		string? token = null,
		string? username = null,
		string? displayName = null,
		bool ignorePasskeyAlreadyExistsError = false,
		bool syncCredentials = false,
		CancellationToken cancellationToken = default)
	{
		using var registration = cancellationToken.Register(native.PasskeyCancel);

		return await native.PasskeySignUpAsync(token, username, displayName, ignorePasskeyAlreadyExistsError, syncCredentials, cancellationToken)
			.ToResponseAsync(AuthsignalJsonContext.Default.SignUpResponse).ConfigureAwait(false);
	}

	public async Task<AuthsignalResponse<SignInResponse>> SignInAsync(
		string? action = null,
		string? token = null,
		bool autofill = false,
		bool preferImmediatelyAvailableCredentials = true,
		bool syncCredentials = false,
		CancellationToken cancellationToken = default)
	{
		using var registration = cancellationToken.Register(native.PasskeyCancel);

		return await native.PasskeySignInAsync(action, token, autofill, preferImmediatelyAvailableCredentials, syncCredentials, cancellationToken)
			.ToResponseAsync(AuthsignalJsonContext.Default.SignInResponse).ConfigureAwait(false);
	}

	public void Cancel() => native.PasskeyCancel();

	public Task<AuthsignalResponse<bool>> ShouldPromptToCreatePasskeyAsync(string? username = null, CancellationToken cancellationToken = default) =>
		native.PasskeyShouldPromptToCreatePasskeyAsync(username, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.Boolean);
}
