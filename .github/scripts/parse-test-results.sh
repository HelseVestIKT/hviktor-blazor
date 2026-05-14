#!/bin/bash
#
# Parses TRX test result files and outputs a summary to GitHub Step Summary or console.
#
# Usage:
#   bash ./parse-test-results.sh [results-path] [output-file]
#
# Parameters:
#   results-path  Path to directory containing TRX files (default: ./TestResults)
#   output-file   Optional path to write summary. Falls back to GITHUB_STEP_SUMMARY or console.

set -euo pipefail

RESULTS_PATH="${1:-./TestResults}"
OUTPUT_FILE="${2:-}"

MAX_ERROR_LENGTH=200

output=""

add_line() {
    output+="${1:-}"$'\n'
}

format_duration_ms() {
    local ms="$1"
    if (( $(echo "$ms >= 60000" | bc -l) )); then
        printf "%.1fm" "$(echo "$ms / 60000" | bc -l)"
    elif (( $(echo "$ms >= 1000" | bc -l) )); then
        printf "%.1fs" "$(echo "$ms / 1000" | bc -l)"
    else
        printf "%.0fms" "$ms"
    fi
}

# Parse ISO 8601 duration (HH:MM:SS.fffffff) to milliseconds
duration_to_ms() {
    local dur="$1"
    if [[ -z "$dur" ]]; then
        echo "0"
        return
    fi
    local h m s
    IFS=':' read -r h m s <<< "$dur"
    h=$((10#${h:-0}))
    m=$((10#${m:-0}))
    local ms
    ms=$(echo "($h * 3600 + $m * 60 + $s) * 1000" | bc -l)
    echo "$ms"
}

format_duration() {
    local dur="$1"
    if [[ -z "$dur" ]]; then
        echo "-"
        return
    fi
    local ms
    ms=$(duration_to_ms "$dur")
    format_duration_ms "$ms"
}

# Extract attribute value from XML element string
get_attr() {
    local xml="$1" attr="$2"
    echo "$xml" | grep -oP "${attr}=\"\K[^\"]*" | head -1
}

# Clean test name: extract last segment and unescape
clean_test_name() {
    local name="$1"
    # Get last dot-segment
    name="${name##*.}"
    # Unescape \" to " and remove backslashes
    name="${name//\\\"/\"}"
    name="${name//\\/}"
    echo "$name"
}

# Extract error message from a TRX file for a specific test (by executionId)
# Uses the region between the UnitTestResult open and close tags
get_error_message() {
    local trx_file="$1" exec_id="$2"
    if [[ -z "$exec_id" ]]; then
        echo ""
        return
    fi
    # Extract the error message between <Message> tags for this execution
    local msg
    msg=$(sed -n "/$exec_id/,/<\/UnitTestResult>/{ /<Message>/,/<\/Message>/p }" "$trx_file" 2>/dev/null | sed 's/<[^>]*>//g' | sed 's/^[[:space:]]*//' | sed 's/[[:space:]]*$//' | sed '/^$/d' | paste -sd'<br>' -)
    if [[ -z "$msg" ]]; then
        echo ""
        return
    fi
    # Replace pipe with fullwidth pipe to avoid breaking markdown tables
    msg="${msg//|/｜}"
    # Replace newline sequences with <br> for markdown table rendering
    msg=$(echo "$msg" | sed 's/[[:space:]]*<br>[[:space:]]*/\<br\>/g; s/[[:space:]]\+/ /g')
    # Truncate
    if [[ ${#msg} -gt $MAX_ERROR_LENGTH ]]; then
        msg="${msg:0:$MAX_ERROR_LENGTH}..."
    fi
    echo "$msg"
}

# Find TRX files
shopt -s nullglob
trx_files=("$RESULTS_PATH"/*.trx)
shopt -u nullglob

if [[ ${#trx_files[@]} -eq 0 ]]; then
    add_line "## ⚠️ No test results found in $RESULTS_PATH"
    echo "$output"
    exit 0
fi

total_passed=0
total_failed=0
total_skipped=0
total_tests=0
total_duration_ms=0

# Temp files for collecting test results
failed_tests_file=$(mktemp)
skipped_tests_file=$(mktemp)
all_tests_file=$(mktemp)
trap 'rm -f "$failed_tests_file" "$skipped_tests_file" "$all_tests_file"' EXIT

for trx_file in "${trx_files[@]}"; do
    echo "Processing: $(basename "$trx_file")"

    # Extract counters
    counters_line=$(grep -oP '<Counters[^/]*/>' "$trx_file" | head -1)
    file_total=$(get_attr "$counters_line" "total")
    file_passed=$(get_attr "$counters_line" "passed")
    file_failed=$(get_attr "$counters_line" "failed")
    file_skipped=$(get_attr "$counters_line" "notExecuted")

    total_tests=$((total_tests + file_total))
    total_passed=$((total_passed + file_passed))
    total_failed=$((total_failed + file_failed))
    total_skipped=$((total_skipped + file_skipped))

    # Extract times for duration
    times_line=$(grep -oP '<Times[^/]*/>' "$trx_file" | head -1)
    start_time=$(get_attr "$times_line" "start")
    finish_time=$(get_attr "$times_line" "finish")

    if [[ -n "$start_time" && -n "$finish_time" ]]; then
        start_epoch=$(date -d "$start_time" +%s%3N 2>/dev/null || echo "0")
        finish_epoch=$(date -d "$finish_time" +%s%3N 2>/dev/null || echo "0")
        file_duration_ms=$((finish_epoch - start_epoch))
        total_duration_ms=$((total_duration_ms + file_duration_ms))
    fi

    # Parse UnitTestResult elements
    while IFS= read -r line; do
        test_name=$(get_attr "$line" "testName")
        outcome=$(get_attr "$line" "outcome")
        duration=$(get_attr "$line" "duration")
        exec_id=$(get_attr "$line" "executionId")
        suite_name="${test_name%.*}"

        echo "${test_name}|${outcome}|${duration}|${suite_name}" >> "$all_tests_file"

        if [[ "$outcome" == "Failed" ]]; then
            error_msg=$(get_error_message "$trx_file" "$exec_id")
            echo "${test_name}|${duration}|${suite_name}|${error_msg}" >> "$failed_tests_file"
        elif [[ "$outcome" == "NotExecuted" ]]; then
            echo "${test_name}" >> "$skipped_tests_file"
        fi
    done < <(grep -oP '<UnitTestResult[^>]*>' "$trx_file")
done

duration_str=$(format_duration_ms "$total_duration_ms")

# Determine summary icon
if [[ $total_failed -eq 0 ]]; then
    summary_icon="✅"
else
    summary_icon="❌"
fi

# Report header
add_line "## $summary_icon Test Results"
add_line ""
add_line "**Tests executed:** $total_tests in $duration_str"
add_line ""
add_line "| Category | Count | Status |"
add_line "|:---------|------:|:-------|"

if [[ $total_passed -eq $total_tests ]]; then
    passed_status="All tests passed"
else
    pct=$(echo "scale=1; $total_passed * 100 / $total_tests" | bc)
    passed_status="${pct}%"
fi
add_line "| ✅ Passed | $total_passed | $passed_status |"

if [[ $total_failed -eq 0 ]]; then
    failed_status="None"
else
    failed_status="See details below"
fi
add_line "| ❌ Failed | $total_failed | $failed_status |"

if [[ $total_skipped -eq 0 ]]; then
    skipped_status="None"
else
    skipped_status="Excluded from run"
fi
add_line "| ⏭️ Skipped | $total_skipped | $skipped_status |"
add_line ""

# Failed tests section
if [[ -s "$failed_tests_file" ]]; then
    fail_count=$(wc -l < "$failed_tests_file")
    add_line "### ❌ Failed Tests ($fail_count)"
    add_line ""
    add_line "| Test | Suite | Time | Error |"
    add_line "|:-----|:------|:-----|:------|"
    while IFS='|' read -r test_name duration suite_name error_msg; do
        clean_name=$(clean_test_name "$test_name")
        short_suite="${suite_name##*.}"
        dur_fmt=$(format_duration "$duration")
        add_line "| $clean_name | $short_suite | $dur_fmt | $error_msg |"
    done < "$failed_tests_file"
    add_line ""
fi

# Test suites summary
declare -A suite_total suite_passed suite_failed suite_skipped suite_duration_ms

while IFS='|' read -r test_name outcome duration suite_name; do
    suite_total["$suite_name"]=$(( ${suite_total["$suite_name"]:-0} + 1 ))
    dur_ms=$(duration_to_ms "$duration")
    suite_duration_ms["$suite_name"]=$(echo "${suite_duration_ms["$suite_name"]:-0} + $dur_ms" | bc -l)

    case "$outcome" in
        Passed) suite_passed["$suite_name"]=$(( ${suite_passed["$suite_name"]:-0} + 1 )) ;;
        Failed) suite_failed["$suite_name"]=$(( ${suite_failed["$suite_name"]:-0} + 1 )) ;;
        NotExecuted) suite_skipped["$suite_name"]=$(( ${suite_skipped["$suite_name"]:-0} + 1 )) ;;
    esac
done < "$all_tests_file"

suite_count=${#suite_total[@]}
add_line "<details>"
add_line "<summary>📊 Test Suites ($suite_count)</summary>"
add_line ""
add_line "| Suite | Tests | Passed | Failed | Skipped | Time |"
add_line "|:------|------:|-------:|-------:|--------:|:-----|"

# Sort suites by failed count descending
sorted_suites=$(for suite in "${!suite_total[@]}"; do
    echo "${suite_failed["$suite"]:-0}|$suite"
done | sort -t'|' -k1 -nr | cut -d'|' -f2-)

while IFS= read -r suite; do
    [[ -z "$suite" ]] && continue
    short_name="${suite##*.}"
    st=${suite_total["$suite"]}
    sp=${suite_passed["$suite"]:-0}
    sf=${suite_failed["$suite"]:-0}
    ss=${suite_skipped["$suite"]:-0}
    sd=$(format_duration_ms "${suite_duration_ms["$suite"]:-0}")
    if [[ $sf -eq 0 ]]; then
        icon="✅"
    else
        icon="❌"
    fi
    add_line "| $icon $short_name | $st | $sp | $sf | $ss | $sd |"
done <<< "$sorted_suites"

add_line ""
add_line "</details>"
add_line ""

# Skipped tests
if [[ -s "$skipped_tests_file" ]]; then
    skip_count=$(wc -l < "$skipped_tests_file")
    add_line "<details>"
    add_line "<summary>⏭️ Skipped Tests ($skip_count)</summary>"
    add_line ""
    add_line '```'
    while IFS= read -r test_name; do
        clean_name=$(clean_test_name "$test_name")
        add_line "$clean_name"
    done < "$skipped_tests_file"
    add_line '```'
    add_line ""
    add_line "</details>"
    add_line ""
fi

# Footer
add_line "---"
add_line ""
add_line '<div align="center">'
add_line '  <sub>Bygget med ❤️ av Helse Vest IKT</sub>'
add_line '</div>'

# Output results
if [[ -n "$OUTPUT_FILE" ]]; then
    echo "$output" >> "$OUTPUT_FILE"
    echo "Test summary written to: $OUTPUT_FILE"
elif [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
    echo "$output" >> "$GITHUB_STEP_SUMMARY"
    echo "Test summary written to: $GITHUB_STEP_SUMMARY"
else
    echo "$output"
fi

echo ""
echo "Test summary generated from ${#trx_files[@]} TRX file(s):"
echo "  Total: $total_tests | Passed: $total_passed | Failed: $total_failed | Skipped: $total_skipped"

