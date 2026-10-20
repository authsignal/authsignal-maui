#!/usr/bin/env bash
set -euo pipefail

cd "$(dirname "$0")"

./gradlew :wrapper:assembleRelease :wrapper:copyBundledDependencies -q
