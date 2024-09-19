using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XStudio.Common
{
    /// <summary>
    /// AES，RSA，MD5，SAH1，SAH256，DES
    /// </summary>
    public enum EncryptionMode
    {
        AES,  
        RSA,     
        DES,
        MD5,
        SAH1,
        SHA256,
        SHA512,
        BASE64,
        HMAC
    }
}
