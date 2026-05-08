using System.Security.Cryptography;

namespace Hviktor.Security;

/// <summary>
/// Utility class for cryptographic operations.
/// </summary>
public static class Cryptography
{
    // Use a power-of-two alphabet (64 chars) to replace expensive % with &
    private const string Table = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_";


    /// <summary>
    /// Generates a unique identifier string.
    /// </summary>
    /// <returns>A string without padding characters, starting with a letter.</returns>
    public static string GenerateId()
    {
        // Single stack allocation
        Span<byte> bytes = stackalloc byte[8];
        RandomNumberGenerator.Fill(bytes);

        // string.Create allocates the string and fills it in-place, avoiding an extra copy.
        return string.Create(8, (ReadOnlySpan<byte>)bytes, (chars, b) =>
        {
            // Mapping 0–255 → 0–51: (randomByte * range) >> 8 shrinks the byte range.
            chars[0] = Table[(b[0] * 52) >> 8];

            // Bit-mask maps 0–255 → 0–63 (power-of-two table size)
            chars[1] = Table[b[1] & 63];
            chars[2] = Table[b[2] & 63];
            chars[3] = Table[b[3] & 63];
            chars[4] = Table[b[4] & 63];
            chars[5] = Table[b[5] & 63];
            chars[6] = Table[b[6] & 63];
            chars[7] = Table[b[7] & 63];
        });
    }

    /// <summary>
    /// Generates a unique identifier string.
    /// </summary>
    /// <param name="length">The number of random bytes to generate.</param>
    /// <returns>A string without padding characters, starting with a letter.</returns>
    /// <remarks>If you're generating a ID of length 8 please use the default GenerateId method instead. It is significantly faster</remarks>
    public static string GenerateId(int length)
    {
        // Stack-allocate for small lengths to avoid heap pressure.
        // The 128-byte threshold keeps us well within typical stack limits (256KB).
        Span<byte> bytes = length <= 128
            ? stackalloc byte[length]
            : new byte[length];

        RandomNumberGenerator.Fill(bytes);

        // string.Create allocates the string and fills it in-place, avoiding an extra copy.
        return string.Create(length, (ReadOnlySpan<byte>)bytes, (chars, b) =>
        {
            // Force first char to be a letter (A–Z or a–z, indices 0–51)
            chars[0] = Table[(b[0] * 52) >> 8];

            for (var i = 1; i < chars.Length; i++)
            {
                // Bit-mask maps 0–255 → 0–63 (power-of-two table size)
                chars[i] = Table[b[i] & 63];
            }
        });
    }
}