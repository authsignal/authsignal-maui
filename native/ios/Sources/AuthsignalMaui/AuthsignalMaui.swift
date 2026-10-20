import Foundation
import Authsignal

public typealias AuthsignalMauiCompletion = (String?, String?, String?) -> Void

@objc(AuthsignalMaui)
public class AuthsignalMaui: NSObject {
  private let passkey: AuthsignalPasskey
  private let email: AuthsignalEmail
  private let sms: AuthsignalSMS
  private let totp: AuthsignalTOTP
  private let whatsapp: AuthsignalWhatsApp
  private let push: AuthsignalPush
  private let qr: AuthsignalQRCode
  private let inapp: AuthsignalInApp
  private let deviceID: String?

  @objc(initWithTenantID:baseURL:deviceID:wrapperVersion:)
  public init(tenantID: String, baseURL: String, deviceID: String?, wrapperVersion: String) {
    AuthsignalRequestMetadata.setWrapperSDK(
      "maui",
      version: wrapperVersion,
      userAgentToken: "AuthsignalMauiSDK"
    )

    self.passkey = AuthsignalPasskey(tenantID: tenantID, baseURL: baseURL, deviceID: deviceID)
    self.email = AuthsignalEmail(tenantID: tenantID, baseURL: baseURL)
    self.sms = AuthsignalSMS(tenantID: tenantID, baseURL: baseURL)
    self.totp = AuthsignalTOTP(tenantID: tenantID, baseURL: baseURL)
    self.whatsapp = AuthsignalWhatsApp(tenantID: tenantID, baseURL: baseURL)
    self.push = AuthsignalPush(tenantID: tenantID, baseURL: baseURL)
    self.qr = AuthsignalQRCode(tenantID: tenantID, baseURL: baseURL)
    self.inapp = AuthsignalInApp(tenantID: tenantID, baseURL: baseURL)
    self.deviceID = deviceID
  }

  @objc(setToken:)
  public func setToken(_ token: String) {
    TokenCache.shared.token = token
  }

  @objc(getDeviceId:)
  public func getDeviceId(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      let deviceID: String

      if let customDeviceID = self.deviceID {
        deviceID = customDeviceID
      } else {
        deviceID = await DeviceCache.shared.getDefaultDeviceID()
      }

      completion(Self.json(deviceID), nil, nil)
    }
  }

  @objc(passkeyIsSupported)
  public func passkeyIsSupported() -> Bool {
    passkey.isSupported()
  }

  @objc(passkeySignUpWithToken:username:displayName:ignorePasskeyAlreadyExistsError:syncCredentials:completion:)
  public func passkeySignUp(
    token: String?,
    username: String?,
    displayName: String?,
    ignorePasskeyAlreadyExistsError: Bool,
    syncCredentials: Bool,
    completion: @escaping AuthsignalMauiCompletion
  ) {
    Task {
      let response = await passkey.signUp(
        token: token,
        username: username,
        displayName: displayName,
        ignorePasskeyAlreadyExistsError: ignorePasskeyAlreadyExistsError,
        syncCredentials: syncCredentials
      )

      Self.complete(response, completion) { Self.object(["token": $0.token]) }
    }
  }

  @objc(passkeySignInWithAction:token:autofill:preferImmediatelyAvailableCredentials:syncCredentials:completion:)
  public func passkeySignIn(
    action: String?,
    token: String?,
    autofill: Bool,
    preferImmediatelyAvailableCredentials: Bool,
    syncCredentials: Bool,
    completion: @escaping AuthsignalMauiCompletion
  ) {
    Task {
      let response = await passkey.signIn(
        token: token,
        action: action,
        autofill: autofill,
        preferImmediatelyAvailableCredentials: preferImmediatelyAvailableCredentials,
        syncCredentials: syncCredentials
      )

      Self.complete(response, completion) {
        Self.object([
          "isVerified": $0.isVerified,
          "token": $0.token,
          "userId": $0.userId,
          "userAuthenticatorId": $0.userAuthenticatorId,
          "username": $0.username,
          "displayName": $0.displayName,
        ])
      }
    }
  }

  @objc(passkeyCancel)
  public func passkeyCancel() {
    passkey.cancel()
  }

  @objc(passkeyShouldPromptToCreatePasskeyWithUsername:completion:)
  public func passkeyShouldPromptToCreatePasskey(username: String?, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      let response = await passkey.shouldPromptToCreatePasskey(username: username)

      completion(Self.json(response.data ?? false), nil, nil)
    }
  }

  @objc(emailEnroll:completion:)
  public func emailEnroll(_ emailAddress: String, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await email.enroll(email: emailAddress), completion, toJSON: Self.enrollObject)
    }
  }

  @objc(emailChallenge:)
  public func emailChallenge(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await email.challenge(), completion, toJSON: Self.challengeObject)
    }
  }

  @objc(emailVerify:completion:)
  public func emailVerify(_ code: String, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await email.verify(code: code), completion, toJSON: Self.verifyObject)
    }
  }

  @objc(smsEnroll:completion:)
  public func smsEnroll(_ phoneNumber: String, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await sms.enroll(phoneNumber: phoneNumber), completion, toJSON: Self.enrollObject)
    }
  }

  @objc(smsChallenge:)
  public func smsChallenge(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await sms.challenge(), completion, toJSON: Self.challengeObject)
    }
  }

  @objc(smsVerify:completion:)
  public func smsVerify(_ code: String, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await sms.verify(code: code), completion, toJSON: Self.verifyObject)
    }
  }

  @objc(totpEnroll:)
  public func totpEnroll(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await totp.enroll(), completion) {
        Self.object([
          "userAuthenticatorId": $0.userAuthenticatorId,
          "uri": $0.uri,
          "secret": $0.secret,
        ])
      }
    }
  }

  @objc(totpVerify:completion:)
  public func totpVerify(_ code: String, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await totp.verify(code: code), completion, toJSON: Self.verifyObject)
    }
  }

  @objc(whatsappChallenge:)
  public func whatsappChallenge(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await whatsapp.challenge(), completion, toJSON: Self.challengeObject)
    }
  }

  @objc(whatsappVerify:completion:)
  public func whatsappVerify(_ code: String, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await whatsapp.verify(code: code), completion, toJSON: Self.verifyObject)
    }
  }

  @objc(pushGetCredential:)
  public func pushGetCredential(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.completeOptional(await push.getCredential(), completion, toJSON: Self.credentialObject)
    }
  }

  @objc(pushAddCredentialWithToken:requireUserAuthentication:keychainAccess:performAttestation:pushToken:completion:)
  public func pushAddCredential(
    token: String?,
    requireUserAuthentication: Bool,
    keychainAccess: String?,
    performAttestation: Bool,
    pushToken: String?,
    completion: @escaping AuthsignalMauiCompletion
  ) {
    Task {
      let response = await push.addCredential(
        token: token,
        keychainAccess: Self.keychainAccess(keychainAccess),
        userPresenceRequired: requireUserAuthentication,
        performAttestation: performAttestation,
        pushToken: pushToken
      )

      Self.complete(response, completion, toJSON: Self.credentialObject)
    }
  }

  @objc(pushRemoveCredential:)
  public func pushRemoveCredential(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await push.removeCredential(), completion) { $0 }
    }
  }

  @objc(pushGetChallenge:)
  public func pushGetChallenge(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.completeOptional(await push.getChallenge(), completion) { challenge in
        challenge.map(Self.challengeObject) ?? NSNull()
      }
    }
  }

  @objc(pushUpdateChallengeWithChallengeId:approved:verificationCode:completion:)
  public func pushUpdateChallenge(
    challengeId: String,
    approved: Bool,
    verificationCode: String?,
    completion: @escaping AuthsignalMauiCompletion
  ) {
    Task {
      let response = await push.updateChallenge(challengeId: challengeId, approved: approved, verificationCode: verificationCode)

      Self.complete(response, completion) { $0 }
    }
  }

  @objc(pushUpdateCredentialWithPushToken:resetExpiry:completion:)
  public func pushUpdateCredential(pushToken: String?, resetExpiry: Bool, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await push.updateCredential(pushToken: pushToken, resetExpiry: resetExpiry), completion) {
        Self.object([
          "userAuthenticatorId": $0.userAuthenticatorId,
          "userId": $0.userId,
          "lastVerifiedAt": $0.lastVerifiedAt,
          "pushToken": $0.pushToken,
          "expiresAt": $0.expiresAt,
        ])
      }
    }
  }

  @objc(qrGetCredential:)
  public func qrGetCredential(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.completeOptional(await qr.getCredential(), completion, toJSON: Self.credentialObject)
    }
  }

  @objc(qrAddCredentialWithToken:requireUserAuthentication:keychainAccess:performAttestation:completion:)
  public func qrAddCredential(
    token: String?,
    requireUserAuthentication: Bool,
    keychainAccess: String?,
    performAttestation: Bool,
    completion: @escaping AuthsignalMauiCompletion
  ) {
    Task {
      let response = await qr.addCredential(
        token: token,
        keychainAccess: Self.keychainAccess(keychainAccess),
        userPresenceRequired: requireUserAuthentication,
        performAttestation: performAttestation
      )

      Self.complete(response, completion, toJSON: Self.credentialObject)
    }
  }

  @objc(qrRemoveCredential:)
  public func qrRemoveCredential(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await qr.removeCredential(), completion) { $0 }
    }
  }

  @objc(qrClaimChallenge:completion:)
  public func qrClaimChallenge(_ challengeId: String, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await qr.claimChallenge(challengeId: challengeId), completion) {
        Self.object([
          "success": $0.success,
          "userAgent": $0.userAgent,
          "ipAddress": $0.ipAddress,
          "actionCode": $0.actionCode,
          "idempotencyKey": $0.idempotencyKey,
          "custom": Self.customObject($0.custom),
          "user": $0.user.map { user in Self.object(["custom": Self.customObject(user.custom)]) },
        ])
      }
    }
  }

  @objc(qrUpdateChallengeWithChallengeId:approved:verificationCode:completion:)
  public func qrUpdateChallenge(
    challengeId: String,
    approved: Bool,
    verificationCode: String?,
    completion: @escaping AuthsignalMauiCompletion
  ) {
    Task {
      let response = await qr.updateChallenge(challengeId: challengeId, approved: approved, verificationCode: verificationCode)

      Self.complete(response, completion) { $0 }
    }
  }

  @objc(inappGetCredentialWithUsername:completion:)
  public func inappGetCredential(username: String?, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.completeOptional(await inapp.getCredential(username: username), completion, toJSON: Self.credentialObject)
    }
  }

  @objc(inappAddCredentialWithToken:requireUserAuthentication:keychainAccess:username:performAttestation:completion:)
  public func inappAddCredential(
    token: String?,
    requireUserAuthentication: Bool,
    keychainAccess: String?,
    username: String?,
    performAttestation: Bool,
    completion: @escaping AuthsignalMauiCompletion
  ) {
    Task {
      let response = await inapp.addCredential(
        token: token,
        keychainAccess: Self.keychainAccess(keychainAccess),
        userPresenceRequired: requireUserAuthentication,
        username: username,
        performAttestation: performAttestation
      )

      Self.complete(response, completion, toJSON: Self.credentialObject)
    }
  }

  @objc(inappRemoveCredentialWithUsername:completion:)
  public func inappRemoveCredential(username: String?, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await inapp.removeCredential(username: username), completion) { $0 }
    }
  }

  @objc(inappVerifyWithAction:username:completion:)
  public func inappVerify(action: String?, username: String?, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await inapp.verify(action: action, username: username), completion) {
        Self.object([
          "token": $0.token,
          "userId": $0.userId,
          "userAuthenticatorId": $0.userAuthenticatorId,
          "username": $0.username,
        ])
      }
    }
  }

  @objc(inappCreatePin:username:token:completion:)
  public func inappCreatePin(_ pin: String, username: String, token: String?, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await inapp.createPin(pin: pin, username: username, token: token), completion, toJSON: Self.credentialObject)
    }
  }

  @objc(inappVerifyPin:username:action:completion:)
  public func inappVerifyPin(_ pin: String, username: String, action: String?, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await inapp.verifyPin(pin: pin, username: username, action: action), completion) {
        Self.object([
          "isVerified": $0.isVerified,
          "token": $0.token,
          "userId": $0.userId,
        ])
      }
    }
  }

  @objc(inappDeletePin:completion:)
  public func inappDeletePin(_ username: String, completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await inapp.deletePin(username: username), completion) { $0 }
    }
  }

  @objc(inappGetAllPinUsernames:)
  public func inappGetAllPinUsernames(_ completion: @escaping AuthsignalMauiCompletion) {
    Task {
      Self.complete(await inapp.getAllPinUsernames(), completion) { $0 }
    }
  }

  private static func keychainAccess(_ value: String?) -> KeychainAccess {
    switch value {
    case "afterFirstUnlock": return .afterFirstUnlock
    case "afterFirstUnlockThisDeviceOnly": return .afterFirstUnlockThisDeviceOnly
    case "whenUnlocked": return .whenUnlocked
    case "whenPasscodeSetThisDeviceOnly": return .whenPasscodeSetThisDeviceOnly
    default: return .whenUnlockedThisDeviceOnly
    }
  }

  private static func credentialObject(_ credential: AppCredential) -> Any {
    object([
      "credentialId": credential.credentialId,
      "createdAt": credential.createdAt,
      "userId": credential.userId,
      "lastAuthenticatedAt": credential.lastAuthenticatedAt,
      "expiresAt": credential.expiresAt,
    ])
  }

  private static func challengeObject(_ challenge: AppChallenge) -> Any {
    object([
      "challengeId": challenge.challengeId,
      "userId": challenge.userId,
      "actionCode": challenge.actionCode,
      "idempotencyKey": challenge.idempotencyKey,
      "userAgent": challenge.userAgent,
      "deviceId": challenge.deviceId,
      "ipAddress": challenge.ipAddress,
      "expiresAt": challenge.expiresAt,
      "custom": customObject(challenge.custom),
      "user": challenge.user.map { user in object(["custom": customObject(user.custom)]) },
    ])
  }

  private static func customObject(_ custom: [String: JSONValue]?) -> Any? {
    custom.map { values in values.mapValues { $0.value ?? NSNull() } }
  }

  private static func enrollObject(_ response: EnrollResponse) -> Any {
    object(["userAuthenticatorId": response.userAuthenticatorId])
  }

  private static func challengeObject(_ response: ChallengeResponse) -> Any {
    object(["challengeId": response.challengeId])
  }

  private static func verifyObject(_ response: VerifyResponse) -> Any {
    object([
      "isVerified": response.isVerified,
      "token": response.token,
      "failureReason": response.failureReason,
    ])
  }

  private static func completeOptional<T>(
    _ response: AuthsignalResponse<T>,
    _ completion: AuthsignalMauiCompletion,
    toJSON: (T) -> Any
  ) {
    if response.error == nil && response.errorCode == "credential_not_found" {
      completion("null", nil, nil)
    } else {
      complete(response, completion, toJSON: toJSON)
    }
  }

  private static func complete<T>(
    _ response: AuthsignalResponse<T>,
    _ completion: AuthsignalMauiCompletion,
    toJSON: (T) -> Any
  ) {
    if response.error != nil || response.errorCode != nil {
      completion(nil, response.errorCode ?? "unexpected_error", response.error)
    } else if let data = response.data {
      completion(json(toJSON(data)), nil, nil)
    } else {
      completion("null", nil, nil)
    }
  }

  private static func object(_ values: [String: Any?]) -> [String: Any] {
    values.compactMapValues { $0 }
  }

  private static func json(_ value: Any) -> String? {
    guard let data = try? JSONSerialization.data(withJSONObject: value, options: .fragmentsAllowed) else {
      return nil
    }

    return String(data: data, encoding: .utf8)
  }
}
