namespace Authsignal.Maui;

public class SignUpResponse
{
	public string? Token { get; init; }
}

public class SignInResponse
{
	public bool IsVerified { get; init; }

	public string? Token { get; init; }

	public string? UserId { get; init; }

	public string? UserAuthenticatorId { get; init; }

	public string? Username { get; init; }

	public string? DisplayName { get; init; }
}

public class EnrollResponse
{
	public required string UserAuthenticatorId { get; init; }
}

public class EnrollTotpResponse
{
	public required string UserAuthenticatorId { get; init; }

	public required string Uri { get; init; }

	public required string Secret { get; init; }
}

public class ChallengeResponse
{
	public required string ChallengeId { get; init; }
}

public class VerifyResponse
{
	public bool IsVerified { get; init; }

	public string? Token { get; init; }

	public string? FailureReason { get; init; }
}
