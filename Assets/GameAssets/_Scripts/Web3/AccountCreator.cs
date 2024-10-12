using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Nethereum.KeyStore;
using Nethereum.Signer;
using Nethereum.Web3.Accounts;
using UnityEngine;

public class AccountCreator
{
    public static Account CreateAccount(string account,string password)
    {
        var fileName = EncryptFileName($"{account}{password}");
        string path = Path.Combine(Application.persistentDataPath, $"{fileName}.json");
        Debug.Log($"Keystore JSON saved at: {path}");
        var keystoreservice = new KeyStoreService();
        if (File.Exists(path))
        {
            string privateKeyHex = BitConverter.ToString(keystoreservice.DecryptKeyStoreFromJson(password, File.ReadAllText(path))).Replace("-", "").ToLower();
            Debug.Log($"Decrypted Private Key: {privateKeyHex}");
            return new Account(privateKeyHex);
        }
        else
        {
            byte[] seed = SHA256Hash(password);
            EthECKey ecKey = new EthECKey(seed, true);
            var address = ecKey.GetPublicAddress();
            var privateKey = ecKey.GetPrivateKeyAsBytes();
            var privateKeyHex = ecKey.GetPrivateKey();
            var keyStoreJson = keystoreservice.EncryptAndGenerateDefaultKeyStoreAsJson(password, privateKey, address);
            Debug.Log($"Account Address: {address}");
            Debug.Log($"Private Key: {privateKeyHex}");
            File.WriteAllText(path, keyStoreJson);
            return new Account(privateKeyHex);
        }
    }

    private static string EncryptFileName(string text)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    }

    private static byte[] SHA256Hash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
    }
}
