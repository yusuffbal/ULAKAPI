using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IRsaCryptingService
    {
        // AES anahtarını belirtilen public anahtarla şifreler
        public byte[] EncryptAesKey(string aesKey, string publicKeySender, string publicKeyReceiver);

        // Şifrelenmiş AES anahtarını belirtilen private anahtarla çözer
        public string DecryptAesKey(byte[] encryptedAesKey, string privateKeySender, string privateKeyReceiver);

        // AES anahtarını her iki kullanıcı için de şifreler
        byte[] EncryptAesKeyForMultipleUsers(string aesKey, string[] publicKeys);


    }
}
