using System.Security.Cryptography;
using System.Text;

namespace QM.BusinessLogic.Service;

public interface IHashService
{
    string HashSha256(string input);
    string HashSha512(string input);
    string HashBcrypt(string input);
    bool VerifyBcrypt(string input, string hash);
    bool VerifySha256(string input, string hash);
}

public class HashService : IHashService
{
    public string HashSha256(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLower();
    }

    public string HashSha512(string input)
    {
        var bytes = SHA512.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLower();
    }

    public string HashBcrypt(string input) =>
        BCrypt.Net.BCrypt.HashPassword(input, workFactor: 12);

    public bool VerifyBcrypt(string input, string hash) =>
        BCrypt.Net.BCrypt.Verify(input, hash);

    public bool VerifySha256(string input, string hash) =>
        HashSha256(input) == hash.ToLower();
}