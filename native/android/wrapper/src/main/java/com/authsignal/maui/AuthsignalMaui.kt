package com.authsignal.maui

import android.app.Activity
import android.app.Application
import android.content.Context
import android.os.Bundle
import com.authsignal.AuthsignalRequestMetadata
import com.authsignal.DeviceCache
import com.authsignal.TokenCache
import com.authsignal.email.AuthsignalEmail
import com.authsignal.inapp.AuthsignalInApp
import com.authsignal.models.AppChallenge
import com.authsignal.models.AppCredential
import com.authsignal.models.ChallengeUser
import com.authsignal.models.AuthsignalResponse
import com.authsignal.models.ChallengeResponse
import com.authsignal.models.EnrollResponse
import com.authsignal.models.VerifyResponse
import com.authsignal.passkey.AuthsignalPasskey
import com.authsignal.push.AuthsignalPush
import com.authsignal.qr.AuthsignalQRCode
import com.authsignal.sms.AuthsignalSMS
import com.authsignal.totp.AuthsignalTOTP
import com.authsignal.whatsapp.AuthsignalWhatsApp
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.SupervisorJob
import kotlinx.coroutines.launch
import kotlinx.serialization.json.JsonObject
import org.json.JSONArray
import org.json.JSONObject

interface AuthsignalMauiCallback {
  fun onComplete(data: String?, errorCode: String?, error: String?)
}

class AuthsignalMaui(
  context: Context,
  private val tenantID: String,
  private val baseURL: String,
  private val deviceID: String?,
  wrapperVersion: String,
) {
  private val scope = CoroutineScope(SupervisorJob() + Dispatchers.Main.immediate)
  private val email = AuthsignalEmail(tenantID, baseURL)
  private val sms = AuthsignalSMS(tenantID, baseURL)
  private val totp = AuthsignalTOTP(tenantID, baseURL)
  private val whatsapp = AuthsignalWhatsApp(tenantID, baseURL)
  private val push = AuthsignalPush(tenantID, baseURL, context.applicationContext)
  private val qr = AuthsignalQRCode(tenantID, baseURL, context.applicationContext)
  private val inapp = AuthsignalInApp(tenantID, baseURL, context.applicationContext)
  private var passkey: AuthsignalPasskey? = null
  private var passkeyActivity: Activity? = null

  init {
    AuthsignalRequestMetadata.setWrapperSDK(
      sdk = "maui",
      version = wrapperVersion,
      userAgentToken = "AuthsignalMauiSDK",
    )

    DeviceCache.shared.initialize(context.applicationContext, deviceID)

    (context.applicationContext as Application).registerActivityLifecycleCallbacks(ActivityReleaser())
  }

  private fun passkey(activity: Activity?): AuthsignalPasskey {
    val current = passkey

    if (current != null && passkeyActivity === activity) {
      return current
    }

    return AuthsignalPasskey(tenantID, baseURL, activity, deviceID).also {
      passkey = it
      passkeyActivity = activity
    }
  }

  private inner class ActivityReleaser : Application.ActivityLifecycleCallbacks {
    override fun onActivityDestroyed(activity: Activity) {
      if (activity === passkeyActivity) {
        passkey = null
        passkeyActivity = null
      }
    }

    override fun onActivityCreated(activity: Activity, savedInstanceState: Bundle?) {}
    override fun onActivityStarted(activity: Activity) {}
    override fun onActivityResumed(activity: Activity) {}
    override fun onActivityPaused(activity: Activity) {}
    override fun onActivityStopped(activity: Activity) {}
    override fun onActivitySaveInstanceState(activity: Activity, outState: Bundle) {}
  }

  fun setToken(token: String) {
    TokenCache.shared.token = token
  }

  fun getDeviceId(callback: AuthsignalMauiCallback) {
    scope.launch {
      callback.onComplete(JSONObject.quote(DeviceCache.shared.getDefaultDeviceId()), null, null)
    }
  }

  fun passkeyIsSupported(): Boolean = (passkey ?: passkey(null)).isSupported()

  fun passkeySignUp(
    activity: Activity?,
    token: String?,
    username: String?,
    displayName: String?,
    ignorePasskeyAlreadyExistsError: Boolean,
    syncCredentials: Boolean,
    callback: AuthsignalMauiCallback,
  ) {
    scope.launch {
      val response = passkey(activity).signUp(
        token = token,
        username = username,
        displayName = displayName,
        ignorePasskeyAlreadyExistsError = ignorePasskeyAlreadyExistsError,
        syncCredentials = syncCredentials,
      )

      complete(response, callback) {
        JSONObject().put("token", it.token)
      }
    }
  }

  fun passkeySignIn(
    activity: Activity?,
    action: String?,
    token: String?,
    preferImmediatelyAvailableCredentials: Boolean,
    syncCredentials: Boolean,
    callback: AuthsignalMauiCallback,
  ) {
    scope.launch {
      val response = passkey(activity).signIn(
        action = action,
        token = token,
        preferImmediatelyAvailableCredentials = preferImmediatelyAvailableCredentials,
        syncCredentials = syncCredentials,
      )

      complete(response, callback) {
        JSONObject()
          .put("isVerified", it.isVerified)
          .put("token", it.token)
          .put("userId", it.userId)
          .put("userAuthenticatorId", it.userAuthenticatorId)
          .put("username", it.username)
          .put("displayName", it.displayName)
      }
    }
  }

  fun passkeyCancel() {}

  fun passkeyShouldPromptToCreatePasskey(username: String?, callback: AuthsignalMauiCallback) {
    scope.launch {
      val response = passkey(null).shouldPromptToCreatePasskey(username)

      callback.onComplete((response.data ?: false).toString(), null, null)
    }
  }

  fun emailEnroll(emailAddress: String, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(email.enroll(emailAddress), callback, ::enrollJson)
    }
  }

  fun emailChallenge(callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(email.challenge(), callback, ::challengeJson)
    }
  }

  fun emailVerify(code: String, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(email.verify(code), callback, ::verifyJson)
    }
  }

  fun smsEnroll(phoneNumber: String, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(sms.enroll(phoneNumber), callback, ::enrollJson)
    }
  }

  fun smsChallenge(callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(sms.challenge(), callback, ::challengeJson)
    }
  }

  fun smsVerify(code: String, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(sms.verify(code), callback, ::verifyJson)
    }
  }

  fun totpEnroll(callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(totp.enroll(), callback) {
        JSONObject()
          .put("userAuthenticatorId", it.userAuthenticatorId)
          .put("uri", it.uri)
          .put("secret", it.secret)
      }
    }
  }

  fun totpVerify(code: String, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(totp.verify(code), callback, ::verifyJson)
    }
  }

  fun whatsappChallenge(callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(whatsapp.challenge(), callback, ::challengeJson)
    }
  }

  fun whatsappVerify(code: String, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(whatsapp.verify(code), callback, ::verifyJson)
    }
  }

  fun pushGetCredential(callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(push.getCredential(), callback, ::credentialJson)
    }
  }

  fun pushAddCredential(token: String?, performAttestation: Boolean, pushToken: String?, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(push.addCredential(token = token, performAttestation = performAttestation, pushToken = pushToken), callback, ::credentialJson)
    }
  }

  fun pushRemoveCredential(callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(push.removeCredential(), callback) { it }
    }
  }

  fun pushGetChallenge(callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(push.getChallenge(), callback) { it?.let(::challengeJson) ?: JSONObject.NULL }
    }
  }

  fun pushUpdateChallenge(challengeId: String, approved: Boolean, verificationCode: String?, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(push.updateChallenge(challengeId, approved, verificationCode), callback) { it }
    }
  }

  fun pushUpdateCredential(pushToken: String?, resetExpiry: Boolean, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(push.updateCredential(pushToken, resetExpiry), callback) {
        JSONObject()
          .put("userAuthenticatorId", it.userAuthenticatorId)
          .put("userId", it.userId)
          .put("lastVerifiedAt", it.lastVerifiedAt)
          .put("pushToken", it.pushToken)
          .put("expiresAt", it.expiresAt)
      }
    }
  }

  fun qrGetCredential(callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(qr.getCredential(), callback, ::credentialJson)
    }
  }

  fun qrAddCredential(token: String?, performAttestation: Boolean, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(qr.addCredential(token = token, performAttestation = performAttestation), callback, ::credentialJson)
    }
  }

  fun qrRemoveCredential(callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(qr.removeCredential(), callback) { it }
    }
  }

  fun qrClaimChallenge(challengeId: String, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(qr.claimChallenge(challengeId), callback) {
        JSONObject()
          .put("success", it.success)
          .put("userAgent", it.userAgent)
          .put("ipAddress", it.ipAddress)
          .put("actionCode", it.actionCode)
          .put("idempotencyKey", it.idempotencyKey)
          .put("custom", customJson(it.custom))
          .put("user", userJson(it.user))
      }
    }
  }

  fun qrUpdateChallenge(challengeId: String, approved: Boolean, verificationCode: String?, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(qr.updateChallenge(challengeId, approved, verificationCode), callback) { it }
    }
  }

  fun inappGetCredential(username: String?, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(inapp.getCredential(username), callback, ::credentialJson)
    }
  }

  fun inappAddCredential(token: String?, username: String?, performAttestation: Boolean, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(inapp.addCredential(token = token, username = username, performAttestation = performAttestation), callback, ::credentialJson)
    }
  }

  fun inappRemoveCredential(username: String?, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(inapp.removeCredential(username = username), callback) { it }
    }
  }

  fun inappVerify(action: String?, username: String?, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(inapp.verify(action, username), callback) {
        JSONObject()
          .put("token", it.token)
          .put("userId", it.userId)
          .put("userAuthenticatorId", it.userAuthenticatorId)
          .put("username", it.username)
      }
    }
  }

  fun inappCreatePin(pin: String, username: String, token: String?, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(inapp.createPin(pin, username, token), callback, ::credentialJson)
    }
  }

  fun inappVerifyPin(pin: String, username: String, action: String?, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(inapp.verifyPin(pin, username, action), callback) {
        JSONObject()
          .put("isVerified", it.isVerified)
          .put("token", it.token)
          .put("userId", it.userId)
      }
    }
  }

  fun inappDeletePin(username: String, callback: AuthsignalMauiCallback) {
    scope.launch {
      complete(inapp.deletePin(username), callback) { it }
    }
  }

  fun inappGetAllPinUsernames(callback: AuthsignalMauiCallback) {
    complete(inapp.getAllPinUsernames(), callback) { JSONArray(it) }
  }

  private fun credentialJson(credential: AppCredential): Any =
    JSONObject()
      .put("credentialId", credential.credentialId)
      .put("createdAt", credential.createdAt)
      .put("userId", credential.userId)
      .put("lastAuthenticatedAt", credential.lastAuthenticatedAt)
      .put("expiresAt", credential.expiresAt)

  private fun challengeJson(challenge: AppChallenge): Any =
    JSONObject()
      .put("challengeId", challenge.challengeId)
      .put("actionCode", challenge.actionCode)
      .put("idempotencyKey", challenge.idempotencyKey)
      .put("userAgent", challenge.userAgent)
      .put("deviceId", challenge.deviceId)
      .put("ipAddress", challenge.ipAddress)
      .put("expiresAt", challenge.expiresAt)
      .put("custom", customJson(challenge.custom))
      .put("user", userJson(challenge.user))

  private fun customJson(custom: JsonObject?): Any? = custom?.let { JSONObject(it.toString()) }

  private fun userJson(user: ChallengeUser?): Any? = user?.let { JSONObject().put("custom", customJson(it.custom)) }

  private fun enrollJson(response: EnrollResponse): Any =
    JSONObject().put("userAuthenticatorId", response.userAuthenticatorId)

  private fun challengeJson(response: ChallengeResponse): Any =
    JSONObject().put("challengeId", response.challengeId)

  private fun verifyJson(response: VerifyResponse): Any =
    JSONObject()
      .put("isVerified", response.isVerified)
      .put("token", response.token)
      .put("failureReason", response.failureReason)

  private fun <T> complete(
    response: AuthsignalResponse<T>,
    callback: AuthsignalMauiCallback,
    toJson: (T) -> Any,
  ) {
    val data = response.data

    if (response.error != null || response.errorCode != null) {
      callback.onComplete(null, response.errorCode ?: "unexpected_error", response.error)
    } else if (data == null) {
      callback.onComplete("null", null, null)
    } else {
      callback.onComplete(toJson(data).toString(), null, null)
    }
  }
}
