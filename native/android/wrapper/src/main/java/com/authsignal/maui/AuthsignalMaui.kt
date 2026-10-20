package com.authsignal.maui

import android.app.Activity
import android.content.Context
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
import kotlinx.coroutines.CancellationException
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
  private val passkeySupport by lazy { passkey(null) }

  init {
    AuthsignalRequestMetadata.setWrapperSDK(
      sdk = "maui",
      version = wrapperVersion,
      userAgentToken = "AuthsignalMauiSDK",
    )

    DeviceCache.shared.initialize(context.applicationContext, deviceID)
  }

  private fun passkey(activity: Activity?) = AuthsignalPasskey(tenantID, baseURL, activity, deviceID)

  private fun run(callback: AuthsignalMauiCallback, block: suspend () -> Unit) {
    scope.launch {
      try {
        block()
      } catch (e: CancellationException) {
        throw e
      } catch (e: Exception) {
        callback.onComplete(null, "unexpected_error", e.message)
      }
    }
  }

  fun setToken(token: String) {
    TokenCache.shared.token = token
  }

  fun getDeviceId(callback: AuthsignalMauiCallback) {
    run(callback) {
      callback.onComplete(JSONObject.quote(DeviceCache.shared.getDefaultDeviceId()), null, null)
    }
  }

  fun passkeyIsSupported(): Boolean = passkeySupport.isSupported()

  fun passkeySignUp(
    activity: Activity?,
    token: String?,
    username: String?,
    displayName: String?,
    ignorePasskeyAlreadyExistsError: Boolean,
    syncCredentials: Boolean,
    callback: AuthsignalMauiCallback,
  ) {
    run(callback) {
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
    run(callback) {
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

  fun passkeyShouldPromptToCreatePasskey(activity: Activity?, username: String?, callback: AuthsignalMauiCallback) {
    run(callback) {
      val response = passkey(activity).shouldPromptToCreatePasskey(username)

      callback.onComplete((response.data ?: false).toString(), null, null)
    }
  }

  fun emailEnroll(emailAddress: String, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(email.enroll(emailAddress), callback, ::enrollJson)
    }
  }

  fun emailChallenge(callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(email.challenge(), callback, ::challengeJson)
    }
  }

  fun emailVerify(code: String, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(email.verify(code), callback, ::verifyJson)
    }
  }

  fun smsEnroll(phoneNumber: String, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(sms.enroll(phoneNumber), callback, ::enrollJson)
    }
  }

  fun smsChallenge(callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(sms.challenge(), callback, ::challengeJson)
    }
  }

  fun smsVerify(code: String, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(sms.verify(code), callback, ::verifyJson)
    }
  }

  fun totpEnroll(callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(totp.enroll(), callback) {
        JSONObject()
          .put("userAuthenticatorId", it.userAuthenticatorId)
          .put("uri", it.uri)
          .put("secret", it.secret)
      }
    }
  }

  fun totpVerify(code: String, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(totp.verify(code), callback, ::verifyJson)
    }
  }

  fun whatsappChallenge(callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(whatsapp.challenge(), callback, ::challengeJson)
    }
  }

  fun whatsappVerify(code: String, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(whatsapp.verify(code), callback, ::verifyJson)
    }
  }

  fun pushGetCredential(callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(push.getCredential(), callback, ::credentialJson)
    }
  }

  fun pushAddCredential(token: String?, performAttestation: Boolean, pushToken: String?, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(push.addCredential(token = token, performAttestation = performAttestation, pushToken = pushToken), callback, ::credentialJson)
    }
  }

  fun pushRemoveCredential(callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(falseIfMissing(push.removeCredential()), callback) { it }
    }
  }

  fun pushGetChallenge(callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(push.getChallenge(), callback) { it?.let(::challengeJson) ?: JSONObject.NULL }
    }
  }

  fun pushUpdateChallenge(challengeId: String, approved: Boolean, verificationCode: String?, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(notFoundIfMissing(push.updateChallenge(challengeId, approved, verificationCode)), callback) { it }
    }
  }

  fun pushUpdateCredential(pushToken: String?, resetExpiry: Boolean, callback: AuthsignalMauiCallback) {
    run(callback) {
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
    run(callback) {
      complete(qr.getCredential(), callback, ::credentialJson)
    }
  }

  fun qrAddCredential(token: String?, performAttestation: Boolean, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(qr.addCredential(token = token, performAttestation = performAttestation), callback, ::credentialJson)
    }
  }

  fun qrRemoveCredential(callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(falseIfMissing(qr.removeCredential()), callback) { it }
    }
  }

  fun qrClaimChallenge(challengeId: String, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(notFoundIfMissing(qr.claimChallenge(challengeId)), callback) {
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
    run(callback) {
      complete(notFoundIfMissing(qr.updateChallenge(challengeId, approved, verificationCode)), callback) { it }
    }
  }

  fun inappGetCredential(username: String?, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(inapp.getCredential(username), callback, ::credentialJson)
    }
  }

  fun inappAddCredential(token: String?, username: String?, performAttestation: Boolean, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(inapp.addCredential(token = token, username = username, performAttestation = performAttestation), callback, ::credentialJson)
    }
  }

  fun inappRemoveCredential(username: String?, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(falseIfMissing(inapp.removeCredential(username = username)), callback) { it }
    }
  }

  fun inappVerify(action: String?, username: String?, callback: AuthsignalMauiCallback) {
    run(callback) {
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
    run(callback) {
      complete(inapp.createPin(pin, username, token), callback, ::credentialJson)
    }
  }

  fun inappVerifyPin(pin: String, username: String, action: String?, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(inapp.verifyPin(pin, username, action), callback) {
        JSONObject()
          .put("isVerified", it.isVerified)
          .put("token", it.token)
          .put("userId", it.userId)
      }
    }
  }

  fun inappDeletePin(username: String, callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(inapp.deletePin(username), callback) { it }
    }
  }

  fun inappGetAllPinUsernames(callback: AuthsignalMauiCallback) {
    run(callback) {
      complete(inapp.getAllPinUsernames(), callback) { JSONArray(it) }
    }
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
      .put("userId", challenge.userId)
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

  private fun falseIfMissing(response: AuthsignalResponse<Boolean>) =
    if (response.isEmpty()) AuthsignalResponse(data = false) else response

  private fun <T> notFoundIfMissing(response: AuthsignalResponse<T>) =
    if (response.isEmpty()) AuthsignalResponse<T>(errorCode = "credential_not_found") else response

  private fun AuthsignalResponse<*>.isEmpty() = data == null && error == null && errorCode == null

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
