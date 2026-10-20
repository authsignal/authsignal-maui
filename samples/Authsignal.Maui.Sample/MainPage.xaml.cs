namespace Authsignal.Maui.Sample;

public partial class MainPage : ContentPage
{
	readonly IAuthsignalClient authsignal;

	public MainPage(IAuthsignalClient authsignal)
	{
		InitializeComponent();

		this.authsignal = authsignal;
	}

	void OnSetTokenClicked(object? sender, EventArgs e)
	{
		authsignal.SetToken(TokenEntry.Text ?? "");
		ResultLabel.Text = "Token set.";
	}

	async void OnPasskeySignUpClicked(object? sender, EventArgs e) =>
		Show(await authsignal.Passkey.SignUpAsync(username: UsernameEntry.Text), _ => "Passkey created.");

	async void OnPasskeySignInClicked(object? sender, EventArgs e) =>
		Show(await authsignal.Passkey.SignInAsync(action: "signInWithPasskey"), data => $"Signed in as {data.Username ?? data.UserId}.");

	async void OnEmailEnrollClicked(object? sender, EventArgs e) =>
		Show(await authsignal.Email.EnrollAsync(EmailEntry.Text ?? ""), _ => "Code sent to your email.");

	async void OnEmailChallengeClicked(object? sender, EventArgs e) =>
		Show(await authsignal.Email.ChallengeAsync(), _ => "Code sent to your email.");

	async void OnEmailVerifyClicked(object? sender, EventArgs e) =>
		Show(await authsignal.Email.VerifyAsync(CodeEntry.Text ?? ""), DescribeVerification);

	async void OnSmsEnrollClicked(object? sender, EventArgs e) =>
		Show(await authsignal.Sms.EnrollAsync(PhoneEntry.Text ?? ""), _ => "Code sent by SMS.");

	async void OnSmsChallengeClicked(object? sender, EventArgs e) =>
		Show(await authsignal.Sms.ChallengeAsync(), _ => "Code sent by SMS.");

	async void OnSmsVerifyClicked(object? sender, EventArgs e) =>
		Show(await authsignal.Sms.VerifyAsync(CodeEntry.Text ?? ""), DescribeVerification);

	async void OnTotpEnrollClicked(object? sender, EventArgs e) =>
		Show(await authsignal.Totp.EnrollAsync(), data => $"Add this key to your authenticator app: {data.Secret}");

	async void OnTotpVerifyClicked(object? sender, EventArgs e) =>
		Show(await authsignal.Totp.VerifyAsync(CodeEntry.Text ?? ""), DescribeVerification);

	// Send the returned token to your backend and validate it there before trusting the result.
	static string DescribeVerification(VerifyResponse data) =>
		data.IsVerified ? "Verified. Validate the returned token on your backend." : $"Not verified: {data.FailureReason}";

	void Show<T>(AuthsignalResponse<T> response, Func<T, string> describe) =>
		ResultLabel.Text = response.Data is { } data ? describe(data) : $"Error: {response.ErrorCode} {response.Error}";
}
