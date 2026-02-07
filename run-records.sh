#!/usr/bin/env bash
set -euo pipefail

# Standalone Records prototype (not the full routed game).

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

dotnet run --project src/records "$@"
