using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAesCryptingService
    {
        public byte[] EncryptMessage(string message, string aesKey);
        public string DecryptMessage(byte[] encryptedMessage, string aesKey);
        public string GenerateAesKey();



    }
}
