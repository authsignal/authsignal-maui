namespace Authsignal.Maui;

public interface IAuthsignalSms
{
	Task<AuthsignalResponse<EnrollResponse>> EnrollAsync(string phoneNumber, CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<ChallengeResponse>> ChallengeAsync(CancellationToken cancellationToken = default);

	Task<AuthsignalResponse<VerifyResponse>> VerifyAsync(string code, CancellationToken cancellationToken = default);
}

sealed class AuthsignalSms(INativeAuthsignal native) : IAuthsignalSms
{
	public Task<AuthsignalResponse<EnrollResponse>> EnrollAsync(string phoneNumber, CancellationToken cancellationToken = default) =>
		native.SmsEnrollAsync(phoneNumber, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.EnrollResponse);

	public Task<AuthsignalResponse<ChallengeResponse>> ChallengeAsync(CancellationToken cancellationToken = default) =>
		native.SmsChallengeAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.ChallengeResponse);

	public Task<AuthsignalResponse<VerifyResponse>> VerifyAsync(string code, CancellationToken cancellationToken = default) =>
		native.SmsVerifyAsync(code, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.VerifyResponse);
}
