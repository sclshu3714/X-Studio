using System.Text;
using System;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Parameters;

namespace XStudio.Helpers
{
    public static class EncrypterHelper
    {
        #region AES加解密算法
        private const string EncryptionKey = "xstudio_web_encryptionkey";// "YourEncryptionKey"; // 替换为您的加密密钥
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
        public static string EncryptAES(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
                Array.Resize(ref key, 32);
                aesAlg.Key = key;
                aesAlg.IV = IV;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
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
                Array.Resize(ref key, 32);
                aesAlg.Key = key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
        #endregion

    }
}
