using System.Text;
using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Parameters;
using System.IO;

namespace XStudio.Helpers
{
    public static class EncrypterHelper
    {
        #region DES加密解密
        public static string EncryptDES(string plainText)
        {
            using (var des = DES.Create())
            {
                byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
                Array.Resize(ref key, 8); // DES 密钥长度为8字节
                des.Key = key;
                des.IV = IV;
                using (var encryptor = des.CreateEncryptor(des.Key, des.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                            cs.Write(plainBytes, 0, plainBytes.Length);
                            cs.Close();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        public static string DecryptDES(string encryptedText)
        {
            using (var des = DES.Create())
            {
                byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
                Array.Resize(ref key, 8); // DES 密钥长度为8字节
                des.Key = key;
                des.IV = IV;
                using (var decryptor = des.CreateDecryptor(des.Key, des.IV))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    using (var ms = new MemoryStream(encryptedBytes))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var reader = new StreamReader(cs))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region AES加解密算法
        public static string EncryptionKey { get; set; } = "xstudio_encryptionkey";// "YourEncryptionKey"; // 替换为您的加密密钥
        private static readonly byte[] IV = new byte[16]; // 使用默认的IV向量
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Encrypt(string plainText)
        {
            try
            {
                byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
                Array.Resize(ref key, 32);
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/PKCS7Padding");
                cipher.Init(true, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", key), IV));
                byte[] encryptedBytes = cipher.DoFinal(plainBytes);
                string encryptedText = Convert.ToBase64String(encryptedBytes);
                return encryptedText;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public static string Decrypt(string encryptedText)
        {
            try
            {
                byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
                Array.Resize(ref key, 32);
                byte[] ciphertext = Convert.FromBase64String(encryptedText);
                IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CTR/PKCS7Padding");
                cipher.Init(false, new ParametersWithIV(ParameterUtilities.CreateKeyParameter("AES", key), IV));
                byte[] plaintext = cipher.DoFinal(ciphertext);
                if (plaintext == null)
                {
                    return "解密错误";
                }
                return System.Text.Encoding.UTF8.GetString(plaintext);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string EncryptAES(string encryptedText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
                byte[] ciphertext = Convert.FromBase64String(encryptedText);
                Array.Resize(ref key, 32);
                aesAlg.Key = key;
                aesAlg.IV = IV;
                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            cs.Write(ciphertext, 0, ciphertext.Length);
                            cs.Close();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public static string DecryptAES(string encryptedText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
                Array.Resize(ref key, 32); // AES 密钥长度为32字节
                aesAlg.Key = key;
                aesAlg.IV = IV;
                using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }
        #endregion


        #region RSA (Rivest-Shamir-Adleman)：常用于加密小数据和数字签名，密钥长度通常为2048位或更高。
        public static string EncryptWithRSA(string encryptedText, RSAParameters publicKey)
        {
            using (var rsa = RSA.Create())
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                rsa.ImportParameters(publicKey);
                return Convert.ToBase64String(rsa.Encrypt(encryptedBytes, RSAEncryptionPadding.OaepSHA256));
            }
        }

        public static string DecryptWithRSA(string decryptText, RSAParameters privateKey)
        {
            using (var rsa = RSA.Create())
            {
                byte[] decryptBytes = Convert.FromBase64String(decryptText);
                rsa.ImportParameters(privateKey);
                return Encoding.UTF8.GetString(rsa.Decrypt(decryptBytes, RSAEncryptionPadding.OaepSHA256));
            }
        }
        #endregion 

        #region MD5加密
        public static string EncryptMD5(string plainText)
        {
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public static bool VerifyMD5(string plainText, string hash)
        {
            string computedHash = EncryptMD5(plainText);
            return computedHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
        #endregion


        #region SHA1加密
        public static string EncryptSHA1(string plainText)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] hashBytes = sha1.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public static bool VerifySHA1(string plainText, string hash)
        {
            string computedHash = EncryptSHA1(plainText);
            return computedHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region SHA-256 (Secure Hash Algorithm)：常用于数据完整性校验和密码存储。
        public static string EncryptSHA256(string plainText)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public static bool VerifySHA256(string plainText, string hash)
        {
            string computedHash = EncryptSHA256(plainText);
            return computedHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region SHA512加密
        public static string EncryptSHA512(string plainText)
        {
            using (var sha512 = SHA512.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] hashBytes = sha512.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public static bool VerifySHA512(string plainText, string hash)
        {
            string computedHash = EncryptSHA512(plainText);
            return computedHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region BASE64编码解码
        public static string EncodeBase64(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainBytes);
        }

        public static string DecodeBase64(string base64EncodedData)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
        #endregion

        #region 使用HMACSHA256
        public static string ComputeHMACSHA256(string data, string key)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
        public static bool VerifyHMACSHA256(string data, string key, string hash)
        {
            string computedHash = ComputeHMACSHA256(data, key);
            return computedHash.Equals(hash, StringComparison.OrdinalIgnoreCase);
        }
        #endregion 

    }
}
