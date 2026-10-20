plugins {
  id("com.android.library")
  id("org.jetbrains.kotlin.android")
}

android {
  namespace = "com.authsignal.maui"
  compileSdk = 35

  defaultConfig {
    minSdk = 23
  }

  compileOptions {
    sourceCompatibility = JavaVersion.VERSION_17
    targetCompatibility = JavaVersion.VERSION_17
  }

  kotlinOptions {
    jvmTarget = "17"
  }
}

dependencies {
  implementation("com.authsignal:authsignal-android:4.3.2")
  implementation("org.jetbrains.kotlinx:kotlinx-coroutines-android:1.10.2")
  implementation("org.jetbrains.kotlinx:kotlinx-serialization-json:1.8.1")
}

// Runtime artifacts without Microsoft-maintained NuGet bindings are bundled unbound in the .NET package.
val bundledGroups = setOf("com.authsignal", "io.ktor", "org.slf4j")
val bundledModules = setOf(
  "kotlinx-io-core-jvm",
  "kotlinx-io-bytestring-jvm",
  "kotlinx-serialization-json-io-jvm",
  "kotlinx-coroutines-slf4j",
)

val bundled: Configuration by configurations.creating {
  isCanBeConsumed = false
  isCanBeResolved = true
  attributes {
    attribute(Usage.USAGE_ATTRIBUTE, objects.named(Usage.JAVA_RUNTIME))
    attribute(Attribute.of("org.jetbrains.kotlin.platform.type", String::class.java), "jvm")
  }
}

dependencies {
  bundled("com.authsignal:authsignal-android:4.3.2")
}

tasks.register<Copy>("copyBundledDependencies") {
  from(provider {
    bundled.resolvedConfiguration.resolvedArtifacts
      .filter { it.moduleVersion.id.group in bundledGroups || it.moduleVersion.id.name in bundledModules }
      .map { it.file }
  })
  into(layout.buildDirectory.dir("bundled"))
}
