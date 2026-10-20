namespace Authsignal.Maui;

/// <summary>
/// SMS OTP enrollment and verification.
/// </summary>
public interface IAuthsignalSms
{
	/// <summary>
	/// Enrolls a phone number and sends a verification code to it.
	/// </summary>
	Task<AuthsignalResponse<EnrollResponse>> EnrollAsync(string phoneNumber, CancellationToken cancellationToken = default);

	/// <summary>
	/// Sends a verification code to the user's enrolled phone number.
	/// </summary>
	Task<AuthsignalResponse<ChallengeResponse>> ChallengeAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Verifies the code sent by SMS.
	/// </summary>
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
