using System.Reflection;

namespace Authsignal.Maui;

/// <summary>
/// Default <see cref="IAuthsignalClient"/> implementation, backed by the Authsignal iOS and Android SDKs.
/// </summary>
public sealed class AuthsignalClient : IAuthsignalClient
{
	internal static readonly string Version =
		typeof(AuthsignalClient).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? "0.0.0";

	readonly INativeAuthsignal native;

	/// <summary>
	/// Creates a client for the given tenant.
	/// </summary>
	public AuthsignalClient(string tenantId, string baseUrl = AuthsignalOptions.DefaultBaseUrl, string? deviceId = null)
		: this(new AuthsignalOptions { TenantId = tenantId, BaseUrl = baseUrl, DeviceId = deviceId })
	{
	}

	/// <summary>
	/// Creates a client from options.
	/// </summary>
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

	/// <inheritdoc />
	public IAuthsignalPasskey Passkey { get; }

	/// <inheritdoc />
	public IAuthsignalEmail Email { get; }

	/// <inheritdoc />
	public IAuthsignalSms Sms { get; }

	/// <inheritdoc />
	public IAuthsignalTotp Totp { get; }

	/// <inheritdoc />
	public IAuthsignalWhatsApp WhatsApp { get; }

	/// <inheritdoc />
	public IAuthsignalPush Push { get; }

	/// <inheritdoc />
	public IAuthsignalQr Qr { get; }

	/// <inheritdoc />
	public IAuthsignalInApp InApp { get; }

	/// <inheritdoc />
	public void SetToken(string token) => native.SetToken(token);

	/// <inheritdoc />
	public async Task<string> GetDeviceIdAsync(CancellationToken cancellationToken = default)
	{
		var response = await native.GetDeviceIdAsync(cancellationToken).ToResponseAsync(AuthsignalJsonContext.Default.String).ConfigureAwait(false);

		return response.Data ?? throw new InvalidOperationException("Unable to get device ID.");
	}

	static AuthsignalOptions Validate(AuthsignalOptions options)
	{
		ArgumentNullException.ThrowIfNull(options);
		ArgumentException.ThrowIfNullOrWhiteSpace(options.TenantId, nameof(options.TenantId));
		ArgumentException.ThrowIfNullOrWhiteSpace(options.BaseUrl, nameof(options.BaseUrl));

		return options;
	}
}
