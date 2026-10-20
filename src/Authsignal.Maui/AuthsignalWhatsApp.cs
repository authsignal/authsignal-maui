namespace Authsignal.Maui;

/// <summary>
/// WhatsApp OTP verification.
/// </summary>
public interface IAuthsignalWhatsApp
{
	/// <summary>
	/// Sends a verification code by WhatsApp to the user's enrolled phone number.
	/// </summary>
	Task<AuthsignalResponse<ChallengeResponse>> ChallengeAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Verifies the code sent by WhatsApp.
	/// </summary>
	Task<AuthsignalResponse<VerifyResponse>> VerifyAsync(string code, CancellationToken cancellationToken = default);
}

sealed class AuthsignalWhatsApp(INativeAuthsignal native) : IAuthsignalWhatsApp
{
	public Task<AuthsignalResponse<ChallengeResponse>> ChallengeAsync(CancellationToken cancellationToken = default) =>
		native.WhatsAppChallengeAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.ChallengeResponse);

	public Task<AuthsignalResponse<VerifyResponse>> VerifyAsync(string code, CancellationToken cancellationToken = default) =>
		native.WhatsAppVerifyAsync(code, cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.VerifyResponse);
}
