#!/usr/bin/env bash
# Builds filter arguments for xUnit v3 test platform from a pipe-separated filter string.
#
# Usage:
#   bash ./build-test-filter-args.sh "Tags=wcag2a|Tags=wcag21a"
#   # Returns: --filter-trait "Tags=wcag2a" --filter-trait "Tags=wcag21a"

set -euo pipefail

filter="${1:?Filter argument is required}"

IFS='|' read -ra parts <<< "$filter"

result=""
for part in "${parts[@]}"; do
  result+="--filter-trait \"$part\" "
done

echo "${result% }"

