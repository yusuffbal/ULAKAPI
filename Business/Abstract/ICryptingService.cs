﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICryptingService
    {
        byte[] EncryptMessage(string message, string aesKey);
        string DecryptMessage(byte[] encryptedMessage, string aesKey);
        string GenerateAesKey();
        public byte[] EncryptAesKey(string aesKey, string publicKeySender, string publicKeyReceiver);
        public string DecryptAesKey(byte[] encryptedAesKey, string privateKeySender, string privateKeyReceiver);
    }
}
