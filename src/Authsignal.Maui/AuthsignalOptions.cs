namespace Authsignal.Maui;

public sealed class AuthsignalOptions
{
	public const string DefaultBaseUrl = "https://api.authsignal.com/v1";

	public string TenantId { get; set; } = "";

	public string BaseUrl { get; set; } = DefaultBaseUrl;

	public string? DeviceId { get; set; }
}
