using Microsoft.Extensions.DependencyInjection;

namespace Authsignal.Maui;

/// <summary>
/// Registers Authsignal with a .NET MAUI app.
/// </summary>
public static class AuthsignalMauiAppBuilderExtensions
{
	/// <summary>
	/// Registers <see cref="IAuthsignalClient"/> as a singleton.
	/// </summary>
	public static MauiAppBuilder UseAuthsignal(this MauiAppBuilder builder, Action<AuthsignalOptions> configure)
	{
		ArgumentNullException.ThrowIfNull(builder);
		ArgumentNullException.ThrowIfNull(configure);

		var options = new AuthsignalOptions();
		configure(options);

		builder.Services.AddSingleton<IAuthsignalClient>(_ => new AuthsignalClient(options));

		return builder;
	}
}
