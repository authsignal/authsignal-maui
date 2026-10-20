namespace Authsignal.Maui;

/// <summary>
/// Email OTP enrollment and verification.
/// </summary>
public interface IAuthsignalEmail
{
	/// <summary>
	/// Enrolls an email address and sends a verification code to it.
	/// </summary>
	Task<AuthsignalResponse<EnrollResponse>> EnrollAsync(string email, CancellationToken cancellationToken = default);

	/// <summary>
	/// Sends a verification code to the user's enrolled email address.
	/// </summary>
	Task<AuthsignalResponse<ChallengeResponse>> ChallengeAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Verifies the code sent by email.
	/// </summary>
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
