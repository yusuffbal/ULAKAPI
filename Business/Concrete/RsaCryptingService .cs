using Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RsaCryptingService : IRsaCryptingService
    {

        // AES anahtarını belirtilen public anahtarla şifreler
        public byte[] EncryptAesKey(string aesKey, string publicKeySender, string publicKeyReceiver)
        {
            try
            {
                // Sender ve receiver için RSA parametrelerini yükleyin
                RSAParameters rsaParametersSender = GetRsaParametersFromXml(publicKeySender);
                RSAParameters rsaParametersReceiver = GetRsaParametersFromXml(publicKeyReceiver);

                using (var rsaSender = new RSACng())
                using (var rsaReceiver = new RSACng())
                {
                    rsaSender.ImportParameters(rsaParametersSender);  // Gönderenin anahtarını yükle
                    rsaReceiver.ImportParameters(rsaParametersReceiver);  // Alıcının anahtarını yükle

                    // AES anahtarını Base64'e çevirmeden önce doğru formatta olduğundan emin olun
                    byte[] aesKeyBytes = Convert.FromBase64String(aesKey);

                    // Gönderenin public key'i ile AES anahtarını şifreleyin
                    byte[] encryptedBySender = rsaSender.Encrypt(aesKeyBytes, RSAEncryptionPadding.OaepSHA256);

                    // Alıcının public key'i ile AES anahtarını şifreleyin
                    byte[] encryptedByReceiver = rsaReceiver.Encrypt(aesKeyBytes, RSAEncryptionPadding.OaepSHA256);

                    // Şifrelenmiş AES anahtarlarını birleştirerek döndürelim (her iki tarafın şifreli versiyonları)
                    byte[] combinedEncryptedKeys = new byte[encryptedBySender.Length + encryptedByReceiver.Length];
                    Buffer.BlockCopy(encryptedBySender, 0, combinedEncryptedKeys, 0, encryptedBySender.Length);
                    Buffer.BlockCopy(encryptedByReceiver, 0, combinedEncryptedKeys, encryptedBySender.Length, encryptedByReceiver.Length);

                    return combinedEncryptedKeys;
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine("Kriptografik hata: " + ex.Message);
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Format hatası: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: " + ex.Message);
                throw;
            }
        }


        // XML formatındaki public key'i RSAParameters'a dönüştüren yardımcı fonksiyon
        private RSAParameters GetRsaParametersFromXml(string publicKey)
        {
            var rsaParameters = new RSAParameters();

            // XML'den modulus ve exponent değerlerini ayıklayın
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(publicKey);

            rsaParameters.Modulus = Convert.FromBase64String(xmlDoc.SelectSingleNode("//Modulus").InnerText);
            rsaParameters.Exponent = Convert.FromBase64String(xmlDoc.SelectSingleNode("//Exponent").InnerText);

            return rsaParameters;
        }


        private RSAParameters GetRsaParametersFromXmlForPrivate(string privateKey)
        {
            var rsaParameters = new RSAParameters();

            // XML'den modulus, exponent, d, p, q, dp, dq ve inverseD değerlerini ayıklayın
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.LoadXml(privateKey);

            rsaParameters.Modulus = Convert.FromBase64String(xmlDoc.SelectSingleNode("//Modulus").InnerText);
            rsaParameters.Exponent = Convert.FromBase64String(xmlDoc.SelectSingleNode("//Exponent").InnerText);
            rsaParameters.D = Convert.FromBase64String(xmlDoc.SelectSingleNode("//D").InnerText);
            rsaParameters.P = Convert.FromBase64String(xmlDoc.SelectSingleNode("//P").InnerText);
            rsaParameters.Q = Convert.FromBase64String(xmlDoc.SelectSingleNode("//Q").InnerText);
            rsaParameters.DP = Convert.FromBase64String(xmlDoc.SelectSingleNode("//DP").InnerText);
            rsaParameters.DQ = Convert.FromBase64String(xmlDoc.SelectSingleNode("//DQ").InnerText);
            rsaParameters.InverseQ = Convert.FromBase64String(xmlDoc.SelectSingleNode("//InverseQ").InnerText);

            return rsaParameters;
        }


        // Şifrelenmiş AES anahtarını belirtilen private anahtarla çözer
        public string DecryptAesKey(byte[] encryptedAesKey, string privateKeySender, string privateKeyReceiver)
        {
            try
            {
                // Sender ve receiver için RSA parametrelerini yükleyin
                RSAParameters rsaParametersSender = GetRsaParametersFromXmlForPrivate(privateKeySender);
                RSAParameters rsaParametersReceiver = GetRsaParametersFromXmlForPrivate(privateKeyReceiver);

                using (var rsaSender = new RSACng())
                using (var rsaReceiver = new RSACng())
                {
                    rsaSender.ImportParameters(rsaParametersSender);  // Gönderenin anahtarını yükle
                    rsaReceiver.ImportParameters(rsaParametersReceiver);  // Alıcının anahtarını yükle

                    // Şifrelenmiş AES anahtarını ikiye bölün (gönderen ve alıcı şifrelemesi)
                    byte[] encryptedBySender = new byte[encryptedAesKey.Length / 2];
                    byte[] encryptedByReceiver = new byte[encryptedAesKey.Length / 2];

                    Buffer.BlockCopy(encryptedAesKey, 0, encryptedBySender, 0, encryptedBySender.Length);
                    Buffer.BlockCopy(encryptedAesKey, encryptedBySender.Length, encryptedByReceiver, 0, encryptedByReceiver.Length);

                    // Gönderenin private key'i ile AES anahtarını çözün
                    byte[] decryptedBySender = rsaSender.Decrypt(encryptedBySender, RSAEncryptionPadding.OaepSHA256);

                    // Alıcının private key'i ile AES anahtarını çözün
                    byte[] decryptedByReceiver = rsaReceiver.Decrypt(encryptedByReceiver, RSAEncryptionPadding.OaepSHA256);

                    // AES anahtarlarını eşleştirip Base64 string olarak geri döndürelim
                    return Convert.ToBase64String(decryptedByReceiver); // Alıcının çözümlediği anahtar döndürülür
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine("Decryption failed due to cryptographic error: " + ex.Message);
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Base64 Format error: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Decryption failed: " + ex.Message);
                return null;  // Veya uygun bir dönüş değeri
            }
        }




        // AES anahtarını her iki kullanıcının public anahtarıyla şifreler
        public byte[] EncryptAesKeyForMultipleUsers(string aesKey, string[] publicKeys)
        {
            byte[] aesKeyBytes = Convert.FromBase64String(aesKey);
            List<byte> encryptedData = new List<byte>();

            foreach (var publicKey in publicKeys)
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlString(publicKey);
                    // RSA ile her kullanıcının public anahtarıyla AES anahtarını şifrele
                    byte[] encryptedKey = rsa.Encrypt(aesKeyBytes, RSAEncryptionPadding.OaepSHA256);
                    encryptedData.AddRange(encryptedKey);
                }
            }
            return encryptedData.ToArray();
        }
    }

}


