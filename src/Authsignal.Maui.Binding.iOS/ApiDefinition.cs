using Foundation;
using ObjCRuntime;

namespace Authsignal.Maui.Binding.iOS;

delegate void AuthsignalMauiCompletion([NullAllowed] string data, [NullAllowed] string errorCode, [NullAllowed] string error);

[BaseType(typeof(NSObject), Name = "AuthsignalMaui")]
[DisableDefaultCtor]
interface AuthsignalMaui
{
	[Export("initWithTenantID:baseURL:deviceID:wrapperVersion:")]
	NativeHandle Constructor(string tenantID, string baseURL, [NullAllowed] string deviceID, string wrapperVersion);

	[Export("setToken:")]
	void SetToken(string token);

	[Export("getDeviceId:")]
	void GetDeviceId(AuthsignalMauiCompletion completion);

	[Export("passkeyIsSupported")]
	bool PasskeyIsSupported();

	[Export("passkeySignUpWithToken:username:displayName:ignorePasskeyAlreadyExistsError:syncCredentials:completion:")]
	void PasskeySignUp([NullAllowed] string token, [NullAllowed] string username, [NullAllowed] string displayName, bool ignorePasskeyAlreadyExistsError, bool syncCredentials, AuthsignalMauiCompletion completion);

	[Export("passkeySignInWithAction:token:preferImmediatelyAvailableCredentials:syncCredentials:completion:")]
	void PasskeySignIn([NullAllowed] string action, [NullAllowed] string token, bool preferImmediatelyAvailableCredentials, bool syncCredentials, AuthsignalMauiCompletion completion);

	[Export("passkeyCancel")]
	void PasskeyCancel();

	[Export("passkeyShouldPromptToCreatePasskeyWithUsername:completion:")]
	void PasskeyShouldPromptToCreatePasskey([NullAllowed] string username, AuthsignalMauiCompletion completion);

	[Export("emailEnroll:completion:")]
	void EmailEnroll(string emailAddress, AuthsignalMauiCompletion completion);

	[Export("emailChallenge:")]
	void EmailChallenge(AuthsignalMauiCompletion completion);

	[Export("emailVerify:completion:")]
	void EmailVerify(string code, AuthsignalMauiCompletion completion);

	[Export("smsEnroll:completion:")]
	void SmsEnroll(string phoneNumber, AuthsignalMauiCompletion completion);

	[Export("smsChallenge:")]
	void SmsChallenge(AuthsignalMauiCompletion completion);

	[Export("smsVerify:completion:")]
	void SmsVerify(string code, AuthsignalMauiCompletion completion);

	[Export("totpEnroll:")]
	void TotpEnroll(AuthsignalMauiCompletion completion);

	[Export("totpVerify:completion:")]
	void TotpVerify(string code, AuthsignalMauiCompletion completion);

	[Export("whatsappChallenge:")]
	void WhatsAppChallenge(AuthsignalMauiCompletion completion);

	[Export("whatsappVerify:completion:")]
	void WhatsAppVerify(string code, AuthsignalMauiCompletion completion);

	[Export("pushGetCredential:")]
	void PushGetCredential(AuthsignalMauiCompletion completion);

	[Export("pushAddCredentialWithToken:requireUserAuthentication:keychainAccess:performAttestation:pushToken:completion:")]
	void PushAddCredential([NullAllowed] string token, bool requireUserAuthentication, [NullAllowed] string keychainAccess, bool performAttestation, [NullAllowed] string pushToken, AuthsignalMauiCompletion completion);

	[Export("pushRemoveCredential:")]
	void PushRemoveCredential(AuthsignalMauiCompletion completion);

	[Export("pushGetChallenge:")]
	void PushGetChallenge(AuthsignalMauiCompletion completion);

	[Export("pushUpdateChallengeWithChallengeId:approved:verificationCode:completion:")]
	void PushUpdateChallenge(string challengeId, bool approved, [NullAllowed] string verificationCode, AuthsignalMauiCompletion completion);

	[Export("pushUpdateCredentialWithPushToken:resetExpiry:completion:")]
	void PushUpdateCredential([NullAllowed] string pushToken, bool resetExpiry, AuthsignalMauiCompletion completion);

	[Export("qrGetCredential:")]
	void QrGetCredential(AuthsignalMauiCompletion completion);

	[Export("qrAddCredentialWithToken:requireUserAuthentication:keychainAccess:performAttestation:completion:")]
	void QrAddCredential([NullAllowed] string token, bool requireUserAuthentication, [NullAllowed] string keychainAccess, bool performAttestation, AuthsignalMauiCompletion completion);

	[Export("qrRemoveCredential:")]
	void QrRemoveCredential(AuthsignalMauiCompletion completion);

	[Export("qrClaimChallenge:completion:")]
	void QrClaimChallenge(string challengeId, AuthsignalMauiCompletion completion);

	[Export("qrUpdateChallengeWithChallengeId:approved:verificationCode:completion:")]
	void QrUpdateChallenge(string challengeId, bool approved, [NullAllowed] string verificationCode, AuthsignalMauiCompletion completion);

	[Export("inappGetCredentialWithUsername:completion:")]
	void InAppGetCredential([NullAllowed] string username, AuthsignalMauiCompletion completion);

	[Export("inappAddCredentialWithToken:requireUserAuthentication:keychainAccess:username:performAttestation:completion:")]
	void InAppAddCredential([NullAllowed] string token, bool requireUserAuthentication, [NullAllowed] string keychainAccess, [NullAllowed] string username, bool performAttestation, AuthsignalMauiCompletion completion);

	[Export("inappRemoveCredentialWithUsername:completion:")]
	void InAppRemoveCredential([NullAllowed] string username, AuthsignalMauiCompletion completion);

	[Export("inappVerifyWithAction:username:completion:")]
	void InAppVerify([NullAllowed] string action, [NullAllowed] string username, AuthsignalMauiCompletion completion);

	[Export("inappCreatePin:username:token:completion:")]
	void InAppCreatePin(string pin, string username, [NullAllowed] string token, AuthsignalMauiCompletion completion);

	[Export("inappVerifyPin:username:action:completion:")]
	void InAppVerifyPin(string pin, string username, [NullAllowed] string action, AuthsignalMauiCompletion completion);

	[Export("inappDeletePin:completion:")]
	void InAppDeletePin(string username, AuthsignalMauiCompletion completion);

	[Export("inappGetAllPinUsernames:")]
	void InAppGetAllPinUsernames(AuthsignalMauiCompletion completion);
}
