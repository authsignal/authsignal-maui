using Microsoft.Extensions.DependencyInjection;

namespace Authsignal.Maui;

public static class AuthsignalMauiAppBuilderExtensions
{
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
