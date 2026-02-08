using System.Security.Cryptography;




namespace ITCompanionAI.Ingestion.Docs;





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








    public static string ComputeSemanticUidForPage(string url)
    {
        // Stable semantic UID for the page (you can change to include language/version)
        return $"learn:{url.Trim().ToLowerInvariant()}";
    }








    public static string ComputeSemanticUidForSection(string pageSemanticUid, string heading, int level, int orderIndex)
    {
        var key = $"{pageSemanticUid}::h{level}:{orderIndex}:{heading}";
        var hash = ComputeSha256(key);
        return "section:" + Convert.ToHexString(hash);
    }








    public static string ComputeSemanticUidForCodeBlock(string pageSemanticUid, string sectionSemanticUid, string language, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        var key = $"{pageSemanticUid}::{sectionSemanticUid}::{language ?? "unknown"}::{content}";
        var hash = ComputeSha256(key);
        return "code:" + Convert.ToHexString(hash);
    }
}