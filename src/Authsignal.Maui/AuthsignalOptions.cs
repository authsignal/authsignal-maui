namespace Authsignal.Maui;

/// <summary>
/// Options for <see cref="AuthsignalClient"/>.
/// </summary>
public sealed class AuthsignalOptions
{
	/// <summary>
	/// The default Authsignal API base URL.
	/// </summary>
	public const string DefaultBaseUrl = "https://api.authsignal.com/v1";

	/// <summary>
	/// Your Authsignal tenant ID.
	/// </summary>
	public string TenantId { get; set; } = "";

	/// <summary>
	/// The Authsignal API base URL for your tenant's region.
	/// </summary>
	public string BaseUrl { get; set; } = DefaultBaseUrl;

	/// <summary>
	/// A custom device ID. When not set, Authsignal generates and stores one.
	/// </summary>
	public string? DeviceId { get; set; }
}
