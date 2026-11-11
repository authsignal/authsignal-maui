using System.Text.Json;

namespace Authsignal.Maui;

public enum KeychainAccess
{
	AfterFirstUnlock,

	AfterFirstUnlockThisDeviceOnly,

	WhenUnlocked,

	WhenUnlockedThisDeviceOnly,

	WhenPasscodeSetThisDeviceOnly,
}

[Flags]
public enum AndroidAuthenticators
{
	BiometricStrong = 1,

	DeviceCredential = 2,
}

public sealed class AndroidUserAuthentication
{
	public int TimeoutSeconds { get; init; } = 15;

	public AndroidAuthenticators Authenticators { get; init; } = AndroidAuthenticators.BiometricStrong | AndroidAuthenticators.DeviceCredential;
}

static class AndroidUserAuthenticationExtensions
{
	public static (int TimeoutSeconds, int AuthorizationType) ToNativeValues(this AndroidUserAuthentication? value, bool requireUserAuthentication)
	{
		if (!requireUserAuthentication)
		{
			return (0, 0);
		}

		value ??= new AndroidUserAuthentication();

		if (value.TimeoutSeconds <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(value), value.TimeoutSeconds, "TimeoutSeconds must be greater than 0.");
		}

		if (value.Authenticators == 0)
		{
			throw new ArgumentOutOfRangeException(nameof(value), value.Authenticators, "At least one authenticator is required.");
		}

		return (value.TimeoutSeconds, (int)value.Authenticators);
	}
}

static class KeychainAccessExtensions
{
	public static string? ToNativeValue(this KeychainAccess? value) => value switch
	{
		KeychainAccess.AfterFirstUnlock => "afterFirstUnlock",
		KeychainAccess.AfterFirstUnlockThisDeviceOnly => "afterFirstUnlockThisDeviceOnly",
		KeychainAccess.WhenUnlocked => "whenUnlocked",
		KeychainAccess.WhenUnlockedThisDeviceOnly => "whenUnlockedThisDeviceOnly",
		KeychainAccess.WhenPasscodeSetThisDeviceOnly => "whenPasscodeSetThisDeviceOnly",
		_ => null,
	};
}

public class AppCredential
{
	public required string CredentialId { get; init; }

	public required string CreatedAt { get; init; }

	public required string UserId { get; init; }

	public string? LastAuthenticatedAt { get; init; }

	public string? ExpiresAt { get; init; }
}

public class UpdatedAppCredential
{
	public required string UserAuthenticatorId { get; init; }

	public required string UserId { get; init; }

	public required string LastVerifiedAt { get; init; }

	public string? PushToken { get; init; }

	public string? ExpiresAt { get; init; }
}

public class ChallengeUser
{
	public Dictionary<string, JsonElement>? Custom { get; init; }
}

public class AppChallenge
{
	public required string ChallengeId { get; init; }

	public string? UserId { get; init; }

	public string? ActionCode { get; init; }

	public string? IdempotencyKey { get; init; }

	public string? UserAgent { get; init; }

	public string? DeviceId { get; init; }

	public string? IpAddress { get; init; }

	public long? ExpiresAt { get; init; }

	public Dictionary<string, JsonElement>? Custom { get; init; }

	public ChallengeUser? User { get; init; }
}

public class ClaimChallengeResponse
{
	public bool Success { get; init; }

	public string? UserAgent { get; init; }

	public string? IpAddress { get; init; }

	public string? ActionCode { get; init; }

	public string? IdempotencyKey { get; init; }

	public Dictionary<string, JsonElement>? Custom { get; init; }

	public ChallengeUser? User { get; init; }
}

public class InAppVerifyResponse
{
	public required string Token { get; init; }

	public required string UserId { get; init; }

	public required string UserAuthenticatorId { get; init; }

	public string? Username { get; init; }
}

public class VerifyPinResponse
{
	public bool IsVerified { get; init; }

	public string? Token { get; init; }

	public string? UserId { get; init; }
}
