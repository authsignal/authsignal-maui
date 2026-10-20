namespace Authsignal.Maui;

public interface IAuthsignalClient
{
	IAuthsignalPasskey Passkey { get; }

	IAuthsignalEmail Email { get; }

	IAuthsignalSms Sms { get; }

	IAuthsignalTotp Totp { get; }

	IAuthsignalWhatsApp WhatsApp { get; }

	IAuthsignalPush Push { get; }

	IAuthsignalQr Qr { get; }

	IAuthsignalInApp InApp { get; }

	void SetToken(string token);

	Task<string> GetDeviceIdAsync(CancellationToken cancellationToken = default);
}
