using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IRsaCryptingService
    {
        public byte[] EncryptAesKey(string aesKey, string publicKeySender, string publicKeyReceiver);

        public string DecryptAesKey(byte[] encryptedAesKey, string privateKeySender, string privateKeyReceiver);

        byte[] EncryptAesKeyForMultipleUsers(string aesKey, string[] publicKeys);


    }
}
