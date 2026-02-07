#!/usr/bin/env bash
set -euo pipefail

# Blessed entrypoint for the game experience.
# Starts the routing runner (Maintenance by default).

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

dotnet run --project src/runner "$@"
