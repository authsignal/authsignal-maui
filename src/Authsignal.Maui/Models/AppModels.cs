using System.Text.Json;

namespace Authsignal.Maui;

/// <summary>
/// iOS keychain accessibility for app credentials. Ignored on Android.
/// </summary>
public enum KeychainAccess
{
	/// <summary>Accessible after the first unlock until restart.</summary>
	AfterFirstUnlock,

	/// <summary>Accessible after the first unlock, on this device only.</summary>
	AfterFirstUnlockThisDeviceOnly,

	/// <summary>Accessible while the device is unlocked.</summary>
	WhenUnlocked,

	/// <summary>Accessible while the device is unlocked, on this device only. The default.</summary>
	WhenUnlockedThisDeviceOnly,

	/// <summary>Accessible only when a passcode is set, on this device only.</summary>
	WhenPasscodeSetThisDeviceOnly,
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

/// <summary>
/// A device credential used for push, QR code or in-app verification.
/// </summary>
public class AppCredential
{
	/// <summary>The ID of the credential.</summary>
	public required string CredentialId { get; init; }

	/// <summary>When the credential was created.</summary>
	public required string CreatedAt { get; init; }

	/// <summary>The ID of the user the credential belongs to.</summary>
	public required string UserId { get; init; }

	/// <summary>When the credential was last used.</summary>
	public string? LastAuthenticatedAt { get; init; }

	/// <summary>When the credential expires, if it expires.</summary>
	public string? ExpiresAt { get; init; }
}

/// <summary>
/// The result of updating a push credential.
/// </summary>
public class UpdatedAppCredential
{
	/// <summary>The ID of the authenticator.</summary>
	public required string UserAuthenticatorId { get; init; }

	/// <summary>The ID of the user.</summary>
	public required string UserId { get; init; }

	/// <summary>When the credential was last verified.</summary>
	public required string LastVerifiedAt { get; init; }

	/// <summary>The push token stored for the credential.</summary>
	public string? PushToken { get; init; }

	/// <summary>When the credential expires, if it expires.</summary>
	public string? ExpiresAt { get; init; }
}

/// <summary>
/// The user associated with a challenge.
/// </summary>
public class ChallengeUser
{
	/// <summary>Custom data for the user.</summary>
	public Dictionary<string, JsonElement>? Custom { get; init; }
}

/// <summary>
/// A pending push challenge.
/// </summary>
public class AppChallenge
{
	/// <summary>The ID of the challenge.</summary>
	public required string ChallengeId { get; init; }

	/// <summary>The action the challenge was created for.</summary>
	public string? ActionCode { get; init; }

	/// <summary>The idempotency key of the action.</summary>
	public string? IdempotencyKey { get; init; }

	/// <summary>The user agent of the device that started the action.</summary>
	public string? UserAgent { get; init; }

	/// <summary>The device ID of the device that started the action.</summary>
	public string? DeviceId { get; init; }

	/// <summary>The IP address of the device that started the action.</summary>
	public string? IpAddress { get; init; }

	/// <summary>When the challenge expires, as a Unix timestamp.</summary>
	public long? ExpiresAt { get; init; }

	/// <summary>Custom data for the challenge.</summary>
	public Dictionary<string, JsonElement>? Custom { get; init; }

	/// <summary>The user the challenge is for.</summary>
	public ChallengeUser? User { get; init; }
}

/// <summary>
/// The result of claiming a QR code challenge.
/// </summary>
public class ClaimChallengeResponse
{
	/// <summary>Whether the challenge was claimed.</summary>
	public bool Success { get; init; }

	/// <summary>The user agent of the device that started the action.</summary>
	public string? UserAgent { get; init; }

	/// <summary>The IP address of the device that started the action.</summary>
	public string? IpAddress { get; init; }

	/// <summary>The action the challenge was created for.</summary>
	public string? ActionCode { get; init; }

	/// <summary>The idempotency key of the action.</summary>
	public string? IdempotencyKey { get; init; }

	/// <summary>Custom data for the challenge.</summary>
	public Dictionary<string, JsonElement>? Custom { get; init; }

	/// <summary>The user the challenge is for.</summary>
	public ChallengeUser? User { get; init; }
}

/// <summary>
/// The result of an in-app verification.
/// </summary>
public class InAppVerifyResponse
{
	/// <summary>A token to validate server-side.</summary>
	public required string Token { get; init; }

	/// <summary>The ID of the verified user.</summary>
	public required string UserId { get; init; }

	/// <summary>The ID of the authenticator used.</summary>
	public required string UserAuthenticatorId { get; init; }

	/// <summary>The username the credential was stored under.</summary>
	public string? Username { get; init; }
}

/// <summary>
/// The result of verifying a PIN.
/// </summary>
public class VerifyPinResponse
{
	/// <summary>Whether the PIN was verified.</summary>
	public bool IsVerified { get; init; }

	/// <summary>A token to validate server-side.</summary>
	public string? Token { get; init; }

	/// <summary>The ID of the verified user.</summary>
	public string? UserId { get; init; }
}
