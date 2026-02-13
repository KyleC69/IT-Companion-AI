using System.Security.Cryptography;




namespace ITCompanionAI.Utilities;





public static class HashUtils
{
    public static byte[] ComputeSha256(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Array.Empty<byte>();
        }

        using SHA256 sha = SHA256.Create();
        return sha.ComputeHash(Encoding.UTF8.GetBytes(input));
    }








    public static string ComputeHash(string input)
    {
        using SHA256 sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));
        return Convert.ToHexString(bytes);
    }








    // For semantic UIDs, we want them to be stable across runs for the same content, so we can use a hash of the content or a stable identifier like URL + heading text.
    public static string ComputeSemanticUidForPage(string path, string language)
    {
        // Path--CodeLang--
        // Stable semantic UID for the page (you can change to include language/version)
        return $"learn:{path.Trim().ToLowerInvariant()}--{language.Trim().ToLowerInvariant()}";
    }








    public static string ComputeSemanticUidForSection(string pageSemanticUid, string heading, int level, int orderIndex)
    {
        var key = $"{pageSemanticUid}::h{level}:{orderIndex}:{heading}";
        var hash = ComputeSha256(key);
        return "section:" + Convert.ToHexString(hash);
    }








    public static string ComputeSemanticUidForCodeBlock(string pageSemanticUid, string sectionSemanticUid, string codelanguage, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        var key = $"{pageSemanticUid}::{sectionSemanticUid}::{codelanguage ?? "unknown"}::{content}";
        var hash = ComputeSha256(key);
        return "code:" + Convert.ToHexString(hash);
    }
}