using System.Security.Cryptography;
using System.Text;

namespace TagR.Application.Common.Hashing;

public static class HashHelper
{
    /// <summary>
    /// Computes the SHA1 hash based on the <paramref name="input"/> parameter.
    /// </summary>
    /// <param name="input">The string input.</param>
    /// <returns>A <see langword="string"/> representation of the computed SHA1 hash.</returns>
    public static string HashSha1(string input)
    {
        using var sha = SHA1.Create();
        var hashBytes = sha.ComputeHash(Encoding.Unicode.GetBytes(input));

        var sb = new StringBuilder();

        for (int i = 0; i < hashBytes.Length; i++)
        {
            sb.Append(hashBytes[i].ToString("X2"));
        }

        return sb.ToString();
    }
}