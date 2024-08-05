using System.Text;
using System;
using SecurityDriven.Inferno;
using System.Security.Cryptography;

namespace XStudio.Helpers
{
    public static class EncrypterHelper
    {
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
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = SuiteB.Encrypt(plainBytes, key);
                return Convert.ToBase64String(encryptedBytes);
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
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = SuiteB.Decrypt(encryptedBytes, key);
                if (decryptedBytes == null)
                {
                    return "解密错误";
                }
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 验证秘钥是否正确
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <returns></returns>
        public static bool Authenticate(string encryptedText)
        {
            byte[] key = Encoding.UTF8.GetBytes(EncryptionKey);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            return SuiteB.Authenticate(key, encryptedBytes);
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
                aesAlg.Key = key;
                aesAlg.IV = IV;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
