namespace Authsignal.Maui;

/// <summary>
/// Client for the Authsignal iOS and Android SDKs.
/// </summary>
public interface IAuthsignalClient
{
	/// <summary>
	/// Passkey sign-up and sign-in.
	/// </summary>
	IAuthsignalPasskey Passkey { get; }

	/// <summary>
	/// Email OTP enrollment and verification.
	/// </summary>
	IAuthsignalEmail Email { get; }

	/// <summary>
	/// SMS OTP enrollment and verification.
	/// </summary>
	IAuthsignalSms Sms { get; }

	/// <summary>
	/// Authenticator app (TOTP) enrollment and verification.
	/// </summary>
	IAuthsignalTotp Totp { get; }

	/// <summary>
	/// WhatsApp OTP verification.
	/// </summary>
	IAuthsignalWhatsApp WhatsApp { get; }

	/// <summary>
	/// Push verification with a device credential.
	/// </summary>
	IAuthsignalPush Push { get; }

	/// <summary>
	/// QR code verification with a device credential.
	/// </summary>
	IAuthsignalQr Qr { get; }

	/// <summary>
	/// In-app and PIN verification with a device credential.
	/// </summary>
	IAuthsignalInApp InApp { get; }

	/// <summary>
	/// Sets the token obtained from a server-side track call or a previous verification.
	/// </summary>
	void SetToken(string token);

	/// <summary>
	/// Gets the device ID used by Authsignal for this app installation.
	/// </summary>
	Task<string> GetDeviceIdAsync(CancellationToken cancellationToken = default);
}
