using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ShiMiao.Common
{
    /// <summary>
    /// DES������
    /// </summary>
    public class DESEncrypt
    {
        private static readonly string oriKey = "shimiao";
        public static class Keys {
            public const string SplitPage = "shimiaopage";
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="text">����</param>
        /// <returns>����</returns>
        public static string Decrypt(string text)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(oriKey));
            byte[] key = new byte[8];
            Array.Copy(buffer, key, key.Length);
            return Decrypt(text, key);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="text">����</param>
        /// <returns>����</returns>
        public static string Decrypt(string text, string key)
        {
            byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key));
            byte[] keys = new byte[8];
            Array.Copy(buffer, keys, keys.Length);
            return Decrypt(text, keys);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="text">����</param>
        /// <param name="key">��Կ</param>
        /// <returns>����</returns>
        public static string Decrypt(string text, byte[] key)
        {
            int len;
            len = text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(key, key), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="text">����</param>
        /// <returns>����</returns>
        public static string Encrypt(string text)
        {
            byte[] buffer = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(oriKey));
            byte[] key = new byte[8];
            Array.Copy(buffer, key, key.Length);
            return Encrypt(text, key);
        }

        public static string Encrypt(string text, string mykey)
        {
            byte[] buffer = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(mykey));
            byte[] key = new byte[8];
            Array.Copy(buffer, key, key.Length);
            return Encrypt(text, key);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="text">����</param>
        /// <param name="key">˽Կ</param>
        /// <returns>����</returns>
        public static string Encrypt(string text, byte[] key)
        {
            byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(key, key), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            StringBuilder ret=new StringBuilder();
            foreach (byte b in mStream.ToArray()) 
			{ 
				ret.AppendFormat("{0:X2}",b); 
			} 
			return ret.ToString(); 
        }
    }
}
