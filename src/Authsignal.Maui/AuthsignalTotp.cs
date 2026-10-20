namespace Authsignal.Maui;

/// <summary>
/// Authenticator app (TOTP) enrollment and verification.
/// </summary>
public interface IAuthsignalTotp
{
	/// <summary>
	/// Starts enrolling an authenticator app.
	/// </summary>
	Task<AuthsignalResponse<EnrollTotpResponse>> EnrollAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Verifies a code from the authenticator app.
	/// </summary>
	Task<AuthsignalResponse<VerifyResponse>> VerifyAsync(string code, CancellationToken cancellationToken = default);
}

sealed class AuthsignalTotp(INativeAuthsignal native) : IAuthsignalTotp
{
	public Task<AuthsignalResponse<EnrollTotpResponse>> EnrollAsync(CancellationToken cancellationToken = default) =>
		native.TotpEnrollAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.EnrollTotpResponse);

	public Task<AuthsignalResponse<VerifyResponse>> VerifyAsync(string code, CancellationToken cancellationToken = default) =>
		native.TotpVerifyAsync(code, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.VerifyResponse);
}
