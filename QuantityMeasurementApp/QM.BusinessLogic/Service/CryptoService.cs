using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace QM.BusinessLogic.Service;

public interface ICryptoService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
}

public class CryptoService : ICryptoService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public CryptoService(IConfiguration config)
    {
        _key = Encoding.UTF8.GetBytes(config["Crypto:Key"]!.PadRight(32)[..32]);
        _iv  = Encoding.UTF8.GetBytes(config["Crypto:IV"]!.PadRight(16)[..16]);
    }

    public string Encrypt(string plainText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV  = _iv;
        var encrypted = aes.EncryptCbc(Encoding.UTF8.GetBytes(plainText), _iv);
        return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string cipherText)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV  = _iv;
        var bytes = aes.DecryptCbc(Convert.FromBase64String(cipherText), _iv);
        return Encoding.UTF8.GetString(bytes);
    }
}