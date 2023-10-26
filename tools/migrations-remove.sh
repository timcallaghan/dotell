#!/usr/bin/env bash
set -euo pipefail

exec 3>&1
function say () {
  printf "%b\n" "[migrations-remove] $1" >&3
}

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
pushd "$SCRIPT_DIR" > /dev/null

say "Removing latest migration from context"

project="../src/DOTelL.DataAccess"
startupProject="../src/DOTelL.Api"

dotnet dotnet-ef migrations remove --force --project "${project}" --startup-project "${startupProject}" --context "SignalDbContext" --verbose
