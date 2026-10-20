namespace Authsignal.Maui;

public class AuthsignalResponse<T>
{
	public T? Data { get; init; }

	public string? Error { get; init; }

	public string? ErrorCode { get; init; }
}
