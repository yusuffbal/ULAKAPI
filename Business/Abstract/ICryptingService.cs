﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICryptingService
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
