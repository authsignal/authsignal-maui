namespace Authsignal.Maui;

public interface IAuthsignalEmail
{
	Task<AuthsignalResponse<EnrollResponse>> EnrollAsync(string email, CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<ChallengeResponse>> ChallengeAsync(CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<VerifyResponse>> VerifyAsync(string code, CancellationToken cancellationToken = default);
}

sealed class AuthsignalEmail(INativeAuthsignal native) : IAuthsignalEmail
{
	public Task<AuthsignalResponse<EnrollResponse>> EnrollAsync(string email, CancellationToken cancellationToken = default) =>
		native.EmailEnrollAsync(email, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.EnrollResponse);

	public Task<AuthsignalResponse<ChallengeResponse>> ChallengeAsync(CancellationToken cancellationToken = default) =>
		native.EmailChallengeAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.ChallengeResponse);

	public Task<AuthsignalResponse<VerifyResponse>> VerifyAsync(string code, CancellationToken cancellationToken = default) =>
		native.EmailVerifyAsync(code, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.VerifyResponse);
}
