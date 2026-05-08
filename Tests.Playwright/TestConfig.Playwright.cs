using System.Text.Json;

namespace Tests.Playwright;

public static class TestConfig
{
    private const string DefaultConfigFile = "testconfig.json";
    private const string LocalConfigFile = "testconfig.Development.json";

    public static class Playwright
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        private static readonly string[] ValidBrowsers = ["chromium", "firefox", "webkit"];

        public static PlaywrightSettings Settings => LazySettings.Value;
        private static readonly Lazy<PlaywrightSettings> LazySettings = new(LoadPlaywrightSettings);

        private static PlaywrightSettings LoadPlaywrightSettings()
        {
            var configFileName = GetConfigFileFromArgs() ?? GetEnvironmentOrDefaultConfigFile();
            var configPath = Path.Combine(AppContext.BaseDirectory, configFileName);

            // Try to load the requested config file
            var settings = TryLoadConfig(configPath);
            if (settings != null)
            {
                return ValidateAndNormalize(settings);
            }

            // Fall back to default config if a different file was requested but failed
            if (configFileName != DefaultConfigFile)
            {
                var defaultPath = Path.Combine(AppContext.BaseDirectory, DefaultConfigFile);
                settings = TryLoadConfig(defaultPath);
                if (settings != null)
                {
                    return ValidateAndNormalize(settings);
                }
            }

            // Use hardcoded defaults as last resort
            return CreateDefaultSettings();
        }

        private static PlaywrightSettings? TryLoadConfig(string configPath)
        {
            if (!File.Exists(configPath))
            {
                return null;
            }

            try
            {
                var json = File.ReadAllText(configPath);
                var config = JsonSerializer.Deserialize<PlaywrightConfig>(json, SerializerOptions);
                return config?.Playwright;
            }
            catch (JsonException)
            {
                return null;
            }
        }

        private static PlaywrightSettings ValidateAndNormalize(PlaywrightSettings settings)
        {
            var browser = settings.Browser.ToLowerInvariant();
            if (!ValidBrowsers.Contains(browser))
            {
                browser = "chromium";
            }

            // Channel is only valid for Chromium
            var channel = browser == "chromium" ? NormalizeChannel(settings.Channel) : null;
            return settings with
            {
                Browser = browser,
                Channel = channel
            };
        }

        private static string? NormalizeChannel(string? channel)
        {
            return string.IsNullOrWhiteSpace(channel) ? null : channel;
        }

        private static string GetEnvironmentOrDefaultConfigFile()
        {
            if (IsRunningOnCi())
            {
                return DefaultConfigFile;
            }

            var localConfigPath = Path.Combine(AppContext.BaseDirectory, LocalConfigFile);
            return File.Exists(localConfigPath)
                ? LocalConfigFile
                : DefaultConfigFile;
        }

        private static string? GetConfigFileFromArgs()
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals("--config-file", StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }

            return null;
        }
    }

    private static bool IsRunningOnCi()
    {
        // Check common CI environment variables
        // CI: Generic (GitHub Actions, GitLab CI, Travis CI, CircleCI, etc.)
        return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI"))
               || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TF_BUILD"))
               || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"))
               || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITLAB_CI"));
    }

    private static PlaywrightSettings CreateDefaultSettings() => new();
}

public record PlaywrightConfig(PlaywrightSettings? Playwright = null);

/// <summary>
/// Settings for Playwright browser automation.
/// </summary>
/// <param name="Headless">Whether to run the browser in headless mode. Default: true</param>
/// <param name="Browser">Browser to use: "chromium", "firefox", or "webkit". Default: "chromium"</param>
/// <param name="Channel">Browser channel (only for Chromium): "chrome", "msedge", "chrome-beta", etc.</param>
/// <param name="SlowMo">Slows down operations by the specified milliseconds. Useful for debugging. Default: 0</param>
/// <param name="Timeout">Maximum time in milliseconds for each action. Default: 30000 (30 seconds)</param>
public record PlaywrightSettings(
    bool Headless = true,
    string Browser = "chromium",
    string? Channel = null,
    int SlowMo = 0,
    int Timeout = 30000);