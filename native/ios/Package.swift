// swift-tools-version: 5.7

import PackageDescription

let package = Package(
  name: "AuthsignalMaui",
  platforms: [
    .iOS(.v15)
  ],
  products: [
    .library(
      name: "AuthsignalMaui",
      type: .dynamic,
      targets: ["AuthsignalMaui"])
  ],
  dependencies: [
    .package(url: "https://github.com/authsignal/authsignal-ios", exact: "2.13.3")
  ],
  targets: [
    .target(
      name: "AuthsignalMaui",
      dependencies: [
        .product(name: "Authsignal", package: "authsignal-ios")
      ])
  ]
)
