#!/bin/bash
# Detects which paths have changed in a Git repository for CI/CD workflows.
#
# Usage:
#   bash ./detect-changed-paths.sh \
#     --base-branch "main" \
#     --before-sha "abc123" \
#     --event-name "pull_request" \
#     --source-code-pattern "^Hviktor/.*\.cs$" \
#     --unit-test-pattern "^Tests\.Unit/.*\.cs$" \
#     --playwright-pattern "^Tests\.Playwright/.*\.cs$" \
#     --output-to-github

set -euo pipefail

# Defaults
BASE_BRANCH=""
BEFORE_SHA=""
EVENT_NAME="pull_request"
SOURCE_CODE_PATTERN=""
UNIT_TEST_PATTERN=""
PLAYWRIGHT_PATTERN=""
OUTPUT_TO_GITHUB=false

# Parse arguments
while [[ $# -gt 0 ]]; do
  case "$1" in
    --base-branch) BASE_BRANCH="$2"; shift 2 ;;
    --before-sha) BEFORE_SHA="$2"; shift 2 ;;
    --event-name) EVENT_NAME="$2"; shift 2 ;;
    --source-code-pattern) SOURCE_CODE_PATTERN="$2"; shift 2 ;;
    --unit-test-pattern) UNIT_TEST_PATTERN="$2"; shift 2 ;;
    --playwright-pattern) PLAYWRIGHT_PATTERN="$2"; shift 2 ;;
    --output-to-github) OUTPUT_TO_GITHUB=true; shift ;;
    *) echo "Unknown argument: $1"; exit 1 ;;
  esac
done

# Get changed files
get_changed_files() {
  local files=""

  if [[ "$EVENT_NAME" == "pull_request" && -n "$BASE_BRANCH" ]]; then
    echo "Comparing against base branch: origin/$BASE_BRANCH" >&2
    files=$(git diff --name-only "origin/$BASE_BRANCH...HEAD" 2>&1) || true
  elif [[ -n "$BEFORE_SHA" && ! "$BEFORE_SHA" =~ ^0+$ ]]; then
    echo "Comparing against before SHA: $BEFORE_SHA" >&2
    files=$(git diff --name-only "$BEFORE_SHA..HEAD" 2>&1) || true
  else
    echo "Comparing against previous commit (HEAD~1)" >&2
    files=$(git diff --name-only HEAD~1 HEAD 2>&1) || true
  fi

  echo "$files"
}

# Test if any file matches a pattern
test_pattern_match() {
  local pattern="$1"
  shift
  local files=("$@")

  if [[ -z "$pattern" ]]; then
    echo "false"
    return
  fi

  for file in "${files[@]}"; do
    if [[ -n "$file" ]] && echo "$file" | grep -qE "$pattern"; then
      echo "true"
      return
    fi
  done

  echo "false"
}

echo "Detecting changed paths..."

# Get changed files into array
mapfile -t CHANGED_FILES < <(get_changed_files | grep -v '^$' || true)
FILE_COUNT=${#CHANGED_FILES[@]}

# Check patterns
SOURCE_CODE_CHANGED=$(test_pattern_match "$SOURCE_CODE_PATTERN" "${CHANGED_FILES[@]+"${CHANGED_FILES[@]}"}")
UNIT_TESTS_CHANGED=$(test_pattern_match "$UNIT_TEST_PATTERN" "${CHANGED_FILES[@]+"${CHANGED_FILES[@]}"}")
PLAYWRIGHT_CHANGED=$(test_pattern_match "$PLAYWRIGHT_PATTERN" "${CHANGED_FILES[@]+"${CHANGED_FILES[@]}"}")

# Write to GitHub Actions output
if [[ "$OUTPUT_TO_GITHUB" == true && -n "${GITHUB_OUTPUT:-}" ]]; then
  echo "source-code=$SOURCE_CODE_CHANGED" >> "$GITHUB_OUTPUT"
  echo "unit-tests=$UNIT_TESTS_CHANGED" >> "$GITHUB_OUTPUT"
  echo "playwright-tests=$PLAYWRIGHT_CHANGED" >> "$GITHUB_OUTPUT"
fi

# Build summary
SUMMARY=""
SUMMARY+="## 🔍 Detected modified files\n\n"
SUMMARY+="**Files analyzed:** $FILE_COUNT\n\n"
SUMMARY+="| Category | Changed | Status |\n"
SUMMARY+="|:---------|:-------:|:-------|\n"

if [[ "$SOURCE_CODE_CHANGED" == "true" ]]; then
  SUMMARY+="| Source Code | ✅ | Will run related jobs |\n"
else
  SUMMARY+="| Source Code | ⏭️ | Skipped |\n"
fi

if [[ "$UNIT_TESTS_CHANGED" == "true" ]]; then
  SUMMARY+="| Unit Tests | ✅ | Will run unit tests |\n"
else
  SUMMARY+="| Unit Tests | ⏭️ | Skipped |\n"
fi

if [[ "$PLAYWRIGHT_CHANGED" == "true" ]]; then
  SUMMARY+="| Playwright Tests | ✅ | Will run Playwright tests |\n"
else
  SUMMARY+="| Playwright Tests | ⏭️ | Skipped |\n"
fi

SUMMARY+="\n"

# Show changed files in collapsible section
if [[ $FILE_COUNT -gt 0 ]]; then
  SUMMARY+="<details>\n"
  SUMMARY+="<summary>📄 Changed files ($FILE_COUNT)</summary>\n\n"
  SUMMARY+="\`\`\`\n"
  for file in "${CHANGED_FILES[@]}"; do
    SUMMARY+="$file\n"
  done
  SUMMARY+="\`\`\`\n\n"
  SUMMARY+="</details>\n\n"
fi

# Output summary
if [[ "$OUTPUT_TO_GITHUB" == true && -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
  echo -e "$SUMMARY" >> "$GITHUB_STEP_SUMMARY"
  echo "Summary written to: $GITHUB_STEP_SUMMARY"
else
  echo -e "$SUMMARY"
fi

echo ""
echo "Detection complete:"
echo "  Source Code: $SOURCE_CODE_CHANGED | Unit Tests: $UNIT_TESTS_CHANGED | Playwright: $PLAYWRIGHT_CHANGED"
