using Business.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CryptingManager : ICryptingService
    {
        private readonly IRsaCryptingService _rsaCryptingService;
        private readonly IAesCryptingService _aesCryptingService;

        public CryptingManager(IRsaCryptingService rsaCryptingService, IAesCryptingService aesCryptingService)
        {
            _rsaCryptingService = rsaCryptingService;
            _aesCryptingService = aesCryptingService;
        }

        public byte[] EncryptMessage(string message, string aesKey)
        {
            return _aesCryptingService.EncryptMessage(message, aesKey);
        }

        public string DecryptMessage(byte[] encryptedMessage, string aesKey)
        {
            return _aesCryptingService.DecryptMessage(encryptedMessage, aesKey);
        }

        public string GenerateAesKey()
        {
            return _aesCryptingService.GenerateAesKey();
        }

        public byte[] EncryptAesKey(string aesKey, string publicKeySender, string publicKeyReceiver)
        {
            return _rsaCryptingService.EncryptAesKey(aesKey, publicKeySender, publicKeyReceiver);
        }

        public string DecryptAesKey(byte[] encryptedAesKey, string privateKeySender, string privateKeyReceiver)
        {
            return _rsaCryptingService.DecryptAesKey(encryptedAesKey, privateKeySender, privateKeyReceiver);
        }
    }
    
}
