using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace Authsignal.Maui;

record NativeResult(string? Data, string? ErrorCode, string? Error)
{
	const string UnexpectedError = "unexpected_error";

	public AuthsignalResponse<T> ToResponse<T>(JsonTypeInfo<T> typeInfo)
	{
		if (ErrorCode != null || Data == null)
		{
			return new AuthsignalResponse<T> { Error = Error, ErrorCode = ErrorCode ?? UnexpectedError };
		}

		try
		{
			return new AuthsignalResponse<T> { Data = JsonSerializer.Deserialize(Data, typeInfo) };
		}
		catch (JsonException ex)
		{
			return new AuthsignalResponse<T> { Error = ex.Message, ErrorCode = UnexpectedError };
		}
	}

	public static async Task<NativeResult> FromCallback(Action<Action<NativeResult>> start, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		var source = new TaskCompletionSource<NativeResult>(TaskCreationOptions.RunContinuationsAsynchronously);

		using var registration = cancellationToken.Register(() => source.TrySetCanceled(cancellationToken));

		start(result => source.TrySetResult(result));

		return await source.Task.ConfigureAwait(false);
	}
}

static class NativeResultExtensions
{
	public static async Task<AuthsignalResponse<T>> ToResponseAsync<T>(this Task<NativeResult> result, JsonTypeInfo<T> typeInfo) =>
		(await result.ConfigureAwait(false)).ToResponse(typeInfo);
}
