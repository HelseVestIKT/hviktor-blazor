#!/bin/bash

PROJECT_NAME="$1"

error() {
  local message="$1"
  echo -e "::error::\033[1;31mError: $message\033[0m" >&2
  return 0
}

info() {
  local message="$1"
  echo -e "::notice::\033[1;34mInfo: $message\033[0m" >&2
  return 0
}

if [[ -z "$PROJECT_NAME" ]]; then
    error "Project name not provided"
    echo "Usage: $0 <project-name>"
    exit 1
fi

# Find the .csproj file
PROJECT_PATH=$(find . -name "$PROJECT_NAME.csproj" -type f | head -n 1)

if [[ -z "$PROJECT_PATH" ]]; then
    error "Could not find '$PROJECT_NAME.csproj' file"
    exit 1
fi

info "Found '$PROJECT_NAME.csproj' file at: '$PROJECT_PATH'"

# Output the path in a format suitable for capturing in GitHub Actions
echo "$PROJECT_PATH"

exit 0