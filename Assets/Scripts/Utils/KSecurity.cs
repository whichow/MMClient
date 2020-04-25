// ***********************************************************************
// Assembly         : Unity
// Author           : King
// Created          : 
//
// Last Modified By : King
// Last Modified On : 
// ***********************************************************************
// <copyright file= "KSecurity" company=""></copyright>
// <summary></summary>
// ***********************************************************************
//#define CRYPTO_ENABLE
namespace Game
{
    using System;
    using System.Text;
    using System.Security.Cryptography;

    public static class KSecurity
    {
        private static int _CryptLength;
        private static string _CryptCode;
        private static byte[] _CryptTable;
        private static byte[] _CryptBuffer;

        private static readonly byte[] _ivKeyData = new byte[1024];
        private static readonly byte[] _ivKeyBuffer = new byte[16];

        public static void SetIVKey(byte[] ivKey)
        {
            if (ivKey != null)
            {
                System.Array.Copy(ivKey, _ivKeyData, 1024);
            }
        }

        public static byte[] GetIVKey(ref int index)
        {
            if (index < 0)
            {
                index = UnityEngine.Random.Range(0, 64);
            }
            System.Array.Copy(_ivKeyData, index << 4, _ivKeyBuffer, 0, 16);
            return _ivKeyBuffer;
        }

        public static string GetString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string GetHexString(this byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            Array.ForEach(bytes, (b) => sb.Append(b.ToString("X2")));
            return sb.ToString();
        }

        public static byte[] GetBytes(this string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        public static byte[] GetMd5(this byte[] bytes)
        {
            return MD5.Create().ComputeHash(bytes);
        }

        public static string GetMd5String(string text)
        {
            var bytes = Encoding.UTF8.GetBytes(text);
            var rBytes = MD5.Create().ComputeHash(bytes);
            return BitConverter.ToString(rBytes).Replace("-", "").ToUpper();
        }

        public static byte[] GetMd5(this string text)
        {
            return GetMd5(GetBytes(text));
        }

        public static byte[] GetSha1(this byte[] bytes)
        {
            return bytes;
            //return new SHA1CryptoServiceProvider().ComputeHash(bytes);
        }

        public static byte[] GetSha1(this string text)
        {
            return GetSha1(GetBytes(text));
        }

        public static string ToBase64S(this string text)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        }

        public static string FromBase64S(this string text)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(text));
        }

        public static byte[] EncryptBytes(byte[] bytes, out string ivKey)
        {
            using (var managed = new RijndaelManaged
            {
                KeySize = 0x80,
                BlockSize = 0x80,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.Zeros
            })
            {
                int key = -1;
                int iv = -1;
                managed.Key = GetIVKey(ref key);
                managed.IV = GetIVKey(ref iv);

                ivKey = string.Format("{0},{1}", key, iv);

                var transform = managed.CreateEncryptor(managed.Key, managed.IV);
                using (var stream = new System.IO.MemoryStream())
                {
                    using (var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                    {
                        stream2.Write(bytes, 0, bytes.Length);
                    }
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        public static byte[] DecryptBytes(byte[] bytes, string ivKey)
        {
            if (!string.IsNullOrEmpty(ivKey))
            {
                using (var managed = new RijndaelManaged
                {
                    KeySize = 0x80,
                    BlockSize = 0x80,
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.Zeros
                })
                {
                    var split = ivKey.Split(',');
                    int key = split[0].ToInt();
                    int iv = split[1].ToInt();

                    managed.Key = GetIVKey(ref key);
                    managed.IV = GetIVKey(ref iv);

                    var transform = managed.CreateDecryptor(managed.Key, managed.IV);
                    using (var stream = new System.IO.MemoryStream())
                    {
                        using (var stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                        {
                            stream2.Write(bytes, 0, bytes.Length);
                        }
                        bytes = stream.ToArray();
                    }
                }
            }
            return bytes;
        }
    }
}
