<img width="1070" alt="Authsignal" src="https://raw.githubusercontent.com/authsignal/authsignal-maui/main/.github/images/authsignal.png">

# Authsignal .NET MAUI SDK

Check out our [official Mobile SDK documentation](https://docs.authsignal.com/sdks/client/mobile/setup).

## Installation

Add the Authsignal .NET MAUI SDK to your project:

```bash
dotnet add package Authsignal.Maui
```

The SDK supports .NET 10 apps targeting iOS 15 and Android API 23 or newer.

## Initialization

Register the Authsignal client in `MauiProgram.cs`:

```csharp
using Authsignal.Maui;

builder.UseAuthsignal(options =>
{
    options.TenantId = "YOUR_TENANT_ID";
    options.BaseUrl = "YOUR_REGION_BASE_URL";
});
```

Then inject `IAuthsignalClient` where you need it. You can also create a client directly:

```csharp
var authsignal = new AuthsignalClient("YOUR_TENANT_ID", "YOUR_REGION_BASE_URL");
```

You can find your tenant ID in the [Authsignal Portal](https://portal.authsignal.com/organisations/tenants/api).

You must specify the correct base URL for your tenant's region.

| Region      | Base URL                         |
| ----------- | -------------------------------- |
| US (Oregon) | https://api.authsignal.com/v1    |
| AU (Sydney) | https://au.api.authsignal.com/v1 |
| EU (Dublin) | https://eu.api.authsignal.com/v1 |

## Usage

### Passkeys

For more detailed info on how to add passkeys to your app using Authsignal, check out our [passkey documentation for Mobile SDKs](https://docs.authsignal.com/authentication-methods/passkey/mobile-sdks).

### App verification

To see how to add app verification with push notifications or QR codes, see our [app verification documentation for Mobile SDKs](https://docs.authsignal.com/authentication-methods/app-verification/push).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
