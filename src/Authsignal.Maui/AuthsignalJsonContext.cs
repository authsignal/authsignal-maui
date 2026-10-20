using System.Text.Json.Serialization;

namespace Authsignal.Maui;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(bool))]
[JsonSerializable(typeof(EnrollTotpResponse))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(AppCredential))]
[JsonSerializable(typeof(UpdatedAppCredential))]
[JsonSerializable(typeof(AppChallenge))]
[JsonSerializable(typeof(ClaimChallengeResponse))]
[JsonSerializable(typeof(InAppVerifyResponse))]
[JsonSerializable(typeof(VerifyPinResponse))]
[JsonSerializable(typeof(SignUpResponse))]
[JsonSerializable(typeof(SignInResponse))]
[JsonSerializable(typeof(EnrollResponse))]
[JsonSerializable(typeof(ChallengeResponse))]
[JsonSerializable(typeof(VerifyResponse))]
internal partial class AuthsignalJsonContext : JsonSerializerContext
{
}
