namespace Authsignal.Maui;

public interface IAuthsignalTotp
{
	Task<AuthsignalResponse<EnrollTotpResponse>> EnrollAsync(CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<VerifyResponse>> VerifyAsync(string code, CancellationToken cancellationToken = default);
}

sealed class AuthsignalTotp(INativeAuthsignal native) : IAuthsignalTotp
{
	public Task<AuthsignalResponse<EnrollTotpResponse>> EnrollAsync(CancellationToken cancellationToken = default) =>
		native.TotpEnrollAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.EnrollTotpResponse);

	public Task<AuthsignalResponse<VerifyResponse>> VerifyAsync(string code, CancellationToken cancellationToken = default) =>
		native.TotpVerifyAsync(code, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.VerifyResponse);
}
