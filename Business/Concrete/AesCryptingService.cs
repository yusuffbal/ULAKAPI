using Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AesCryptingService :IAesCryptingService
    {
        public byte[] EncryptMessage(string message, string aesKey)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(aesKey);
                aesAlg.GenerateIV();

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                using (var ms = new MemoryStream())
                {
                    ms.Write(aesAlg.IV, 0, aesAlg.IV.Length); 
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(message);
                    }

                    return ms.ToArray();
                }
            }
        }

        public string DecryptMessage(byte[] encryptedMessage, string aesKey)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = Convert.FromBase64String(aesKey);
                byte[] iv = encryptedMessage.Take(aesAlg.BlockSize / 8).ToArray();  // IV'yi al

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, iv))
                using (var ms = new MemoryStream(encryptedMessage.Skip(aesAlg.BlockSize / 8).ToArray()))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public string GenerateAesKey()
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                return Convert.ToBase64String(aesAlg.Key);
            }
        }
    }
}
