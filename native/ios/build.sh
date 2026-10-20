#!/usr/bin/env bash
set -euo pipefail

cd "$(dirname "$0")"

DERIVED_DATA="build/DerivedData"
OUTPUT="build/AuthsignalMaui.xcframework"

for destination in "iOS" "iOS Simulator"; do
  xcodebuild build \
    -scheme AuthsignalMaui \
    -destination "generic/platform=$destination" \
    -derivedDataPath "$DERIVED_DATA" \
    -configuration Release \
    -quiet
done

rm -rf "$OUTPUT"

xcodebuild -create-xcframework \
  -framework "$DERIVED_DATA/Build/Products/Release-iphoneos/PackageFrameworks/AuthsignalMaui.framework" \
  -framework "$DERIVED_DATA/Build/Products/Release-iphonesimulator/PackageFrameworks/AuthsignalMaui.framework" \
  -output "$OUTPUT"
