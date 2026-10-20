using System.Reflection;

namespace Authsignal.Maui;

public sealed class AuthsignalClient : IAuthsignalClient
{
	internal static readonly string Version =
		typeof(AuthsignalClient).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? "0.0.0";

	readonly INativeAuthsignal native;

	public AuthsignalClient(string tenantId, string baseUrl = AuthsignalOptions.DefaultBaseUrl, string? deviceId = null)
		: this(new AuthsignalOptions { TenantId = tenantId, BaseUrl = baseUrl, DeviceId = deviceId })
	{
	}

	public AuthsignalClient(AuthsignalOptions options)
		: this(new NativeAuthsignal(Validate(options)))
	{
	}

	internal AuthsignalClient(INativeAuthsignal native)
	{
		this.native = native;
		Passkey = new AuthsignalPasskey(native);
		Email = new AuthsignalEmail(native);
		Sms = new AuthsignalSms(native);
		Totp = new AuthsignalTotp(native);
		WhatsApp = new AuthsignalWhatsApp(native);
		Push = new AuthsignalPush(native);
		Qr = new AuthsignalQr(native);
		InApp = new AuthsignalInApp(native);
	}

	public IAuthsignalPasskey Passkey { get; }

	public IAuthsignalEmail Email { get; }

	public IAuthsignalSms Sms { get; }

	public IAuthsignalTotp Totp { get; }

	public IAuthsignalWhatsApp WhatsApp { get; }

	public IAuthsignalPush Push { get; }

	public IAuthsignalQr Qr { get; }

	public IAuthsignalInApp InApp { get; }

	public void SetToken(string token) => native.SetToken(token);

	public async Task<string> GetDeviceIdAsync(CancellationToken cancellationToken = default)
	{
		var response = await native.GetDeviceIdAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.String).ConfigureAwait(false);

		return response.Data ?? throw new InvalidOperationException("Unable to get device ID.");
	}

	internal static AuthsignalOptions Validate(AuthsignalOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentException.ThrowIfNullOrWhiteSpace(options.TenantId, nameof(options.TenantId));
		ArgumentException.ThrowIfNullOrWhiteSpace(options.BaseUrl, nameof(options.BaseUrl));

		return options;
	}
}
