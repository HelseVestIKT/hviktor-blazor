# **WikiGen**

CLI tool that generates GitHub Wiki markdown pages from the `ComponentRegistry` and XML documentation.

## Usage

```bash
# Output to default ../Hviktor Wiki/
dotnet run --project WikiGen

# Output to custom directory
dotnet run --project WikiGen -- "path/to/output"
```

The tool reads the same `ComponentRegistry`, metadata services, and demo source files used by the Documentation site,
then produces one `.md` file per component in the output directory.

## How It Works

1. Resolves component metadata (parameters, XML docs) via `ComponentMetadataService`.
2. Reads demo `.razor` source files from disk via `FileBasedDemoSourceService`.
3. Builds markdown using `WikiMarkdownBuilder`.
4. Writes all pages to the output directory via `WikiGeneratorService`.

## Project References

- `Documentation` – reuses `ComponentRegistry`, `IComponentMetadataService`, and `IDemoSourceService`.
