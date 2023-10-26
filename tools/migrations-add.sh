#!/usr/bin/env bash
set -euo pipefail

exec 3>&1
function say () {
  printf "%b\n" "[migrations-add] $1" >&3
}

function say_err() {
  if [ -t 1 ] && command -v tput > /dev/null; then
    RED='\033[0;31m'
    NC='\033[0m' # No Color
  fi
  printf "%b\n" "${RED:-}[migrations-add] Error: $1${NC:-}" >&2
}

SCRIPT_DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" >/dev/null 2>&1 && pwd )"
pushd "$SCRIPT_DIR" > /dev/null

while [ "$#" -gt 0 ]; do
  case "$1" in
    --name=*) name="${1#*=}"; shift 1;;

    # validation
    --name \
    ) say_err "$1 requires an argument" >&2; exit 1;;

    # help
    -h|--help|-[Hh]elp|-?|--?)
      script_name="$(basename "$0")"
      say "Usage:"
      say "$script_name [--name=<string>]"
      exit 0
      ;;
    -*) say_err "unknown option: $1"; exit 1;;
    *) say_err "unknown argument: $1"; exit 1;;
  esac
done

say "Adding migration ${name}"

project="../src/DOTelL.DataAccess"
startupProject="../src/DOTelL.Api"

dotnet dotnet-ef migrations add "${name}" --project "${project}" --startup-project "${startupProject}" --context "SignalDbContext" --verbose