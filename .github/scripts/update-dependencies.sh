#!/bin/bash

PACKAGE_VERSION="$1"
SOURCE="$2"
PROJECT_CSPROJ="$3"

warning() {
  local message="$1"
  echo -e "::warning::\033[1;33mWarning: $message\033[0m" >&2
  return 0
}

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

# Get the current project name from environment variable
if [[ -z "$project" ]]; then
    error "'project' environment variable not set"
    exit 1
fi

echo "Updating dependencies for project: $project"

if [[ -z "$PROJECT_CSPROJ" ]]; then
    error "Invalid or missing project file path"
    exit 1
fi

# Find Directory.Packages.props by walking up from the project file
PROJECT_DIR=$(dirname "$PROJECT_CSPROJ")
PACKAGES_PROPS=""
SEARCH_DIR="$PROJECT_DIR"
while [[ "$SEARCH_DIR" != "/" ]]; do
    if [[ -f "$SEARCH_DIR/Directory.Packages.props" ]]; then
        PACKAGES_PROPS="$SEARCH_DIR/Directory.Packages.props"
        break
    fi
    SEARCH_DIR=$(dirname "$SEARCH_DIR")
done

if [[ -z "$PACKAGES_PROPS" ]]; then
    error "Directory.Packages.props not found"
    exit 1
fi

info "Using Central Package Management: $PACKAGES_PROPS"

# Extract all Hviktor package dependencies from the current project
mapfile -t DEPENDENCIES < <(grep -o '<PackageReference Include="Hviktor[^"]*"' "$PROJECT_CSPROJ" | sed 's/<PackageReference Include="\(.*\)"/\1/')

# Update each dependency version in Directory.Packages.props
for DEP in "${DEPENDENCIES[@]}"; do
    # Skip self-references to avoid circular dependencies
    if [[ "$DEP" = "$project" ]]; then
        warning "Skipping self-reference to $DEP"
        continue
    fi

    info "Updating dependency: $DEP to version $PACKAGE_VERSION in Directory.Packages.props"

    if grep -q "Include=\"$DEP\"" "$PACKAGES_PROPS"; then
        sed -i "s|<PackageVersion Include=\"$DEP\" Version=\"[^\"]*\"|<PackageVersion Include=\"$DEP\" Version=\"$PACKAGE_VERSION\"|" "$PACKAGES_PROPS"
    else
        # Package not yet in central file; add before closing ItemGroup
        sed -i "/<\/ItemGroup>/i\\        <PackageVersion Include=\"$DEP\" Version=\"$PACKAGE_VERSION\"/>" "$PACKAGES_PROPS"
        warning "Added new entry for $DEP in Directory.Packages.props"
    fi
done

exit 0