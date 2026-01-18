using System.Security.Cryptography;





public static class HashUtil
{
    public static string ComputeHash(string input)
    {
        using SHA256 sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input ?? string.Empty));
        return Convert.ToHexString(bytes);
    }
}