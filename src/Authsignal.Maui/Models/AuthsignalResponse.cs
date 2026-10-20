namespace Authsignal.Maui;

/// <summary>
/// The result of an Authsignal call. Either <see cref="Data"/> or <see cref="ErrorCode"/> is set.
/// </summary>
public class AuthsignalResponse<T>
{
	/// <summary>
	/// The response data when the call succeeded.
	/// </summary>
	public T? Data { get; init; }

	/// <summary>
	/// A description of the error when the call failed.
	/// </summary>
	public string? Error { get; init; }

	/// <summary>
	/// The error code when the call failed, for example <c>token_not_set</c> or <c>invalid_token</c>.
	/// </summary>
	public string? ErrorCode { get; init; }
}
