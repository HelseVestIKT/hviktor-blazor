namespace Hviktor.Extensions.Text;

/// <summary>
/// Extension methods for string operations.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Splits a string into segments based on capitalization.
    /// </summary>
    /// <param name="input">The string to split.</param>
    /// <returns>A list of segments, where each segment is a contiguous sequence of uppercase letters.</returns>
    /// <remarks>
    /// This method is "allocation-light".<br/>
    /// It only creates the final strings required for a list.<br/>
    /// It is "secure" in that it prevents common buffer-related vulnerabilities by using type-safe memory access. 
    /// <br/><br/><b>Note on Acronyms:</b><br/>
    /// If your input contains acronyms (e.g., "JSONParser"), the logic splits it as ["JSONParser"],
    /// not as ["JSON", "Parser"]. This is intentional to avoid splitting on acronyms.<br/>
    /// If your input contains mixed capitalization (e.g., "JsonParser"), the logic splits it as ["Json", "Parser"].
    /// </remarks>
    public static List<string> SplitByCapitalization(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return [];
        }

        var result = new List<string>();
        var span = input.AsSpan();
        var start = 0;

        for (var i = 1; i < span.Length; i++)
        {
            // Detect transition: current char is Upper AND previous char is Lower
            // This handles standard CamelCase and PascalCase
            if (char.IsUpper(span[i]) && char.IsLower(span[i - 1]))
            {
                result.Add(span[start..i].ToString());
                start = i;
            }
        }

        // Add the final segment
        result.Add(span[start..].ToString());
        return result;
    }
}