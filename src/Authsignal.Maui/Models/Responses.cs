namespace Authsignal.Maui;

/// <summary>
/// The result of a passkey sign-up.
/// </summary>
public class SignUpResponse
{
	/// <summary>
	/// A token to validate server-side.
	/// </summary>
	public string? Token { get; init; }
}

/// <summary>
/// The result of a passkey sign-in.
/// </summary>
public class SignInResponse
{
	/// <summary>
	/// Whether the passkey was verified.
	/// </summary>
	public bool IsVerified { get; init; }

	/// <summary>
	/// A token to validate server-side.
	/// </summary>
	public string? Token { get; init; }

	/// <summary>
	/// The ID of the signed-in user.
	/// </summary>
	public string? UserId { get; init; }

	/// <summary>
	/// The ID of the passkey authenticator.
	/// </summary>
	public string? UserAuthenticatorId { get; init; }

	/// <summary>
	/// The user's username.
	/// </summary>
	public string? Username { get; init; }

	/// <summary>
	/// The user's display name.
	/// </summary>
	public string? DisplayName { get; init; }
}

/// <summary>
/// The result of an enrollment.
/// </summary>
public class EnrollResponse
{
	/// <summary>
	/// The ID of the enrolled authenticator.
	/// </summary>
	public required string UserAuthenticatorId { get; init; }
}

/// <summary>
/// The result of enrolling an authenticator app.
/// </summary>
public class EnrollTotpResponse
{
	/// <summary>
	/// The ID of the enrolled authenticator.
	/// </summary>
	public required string UserAuthenticatorId { get; init; }

	/// <summary>
	/// The otpauth URI to show as a QR code.
	/// </summary>
	public required string Uri { get; init; }

	/// <summary>
	/// The secret for manual entry in an authenticator app.
	/// </summary>
	public required string Secret { get; init; }
}

/// <summary>
/// The result of sending a challenge.
/// </summary>
public class ChallengeResponse
{
	/// <summary>
	/// The ID of the challenge.
	/// </summary>
	public required string ChallengeId { get; init; }
}

/// <summary>
/// The result of verifying a code.
/// </summary>
public class VerifyResponse
{
	/// <summary>
	/// Whether the code was verified.
	/// </summary>
	public bool IsVerified { get; init; }

	/// <summary>
	/// A token to validate server-side.
	/// </summary>
	public string? Token { get; init; }

	/// <summary>
	/// Why verification failed, for example <c>CODE_INVALID_OR_EXPIRED</c>.
	/// </summary>
	public string? FailureReason { get; init; }
}
