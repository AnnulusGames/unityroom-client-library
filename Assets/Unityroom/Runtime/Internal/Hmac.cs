using System;
using System.Text;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

[assembly: InternalsVisibleTo("Unityroom.Tests.Editor")]

namespace Unityroom.Internal
{
    internal static class Hmac
    {
        /// <summary>
        /// SHA256を用いてHMACを計算する
        /// </summary>
        /// <param name="dataText">計算対象文字列</param>
        /// <param name="base64AuthenticationKey">HMACSHA256に用いるキー（base64）</param>
        /// <returns>HMAC SHA-256</returns>
        public static string GetHmacSha256(string dataText, string base64AuthenticationKey)
        {
            var dataBytes = encoding.GetBytes(dataText); // BOM無しUTF-8のbyte配列に変換
            var keyBytes = Convert.FromBase64String(base64AuthenticationKey); // base64の認証用キーをデコードしてbyte配列に変換
            var sha256 = new HMACSHA256(keyBytes); // ハッシュ関数はSHA256を用いる
            var hmacBytes = sha256.ComputeHash(dataBytes); // ハッシュ値を計算
            var hmacText = BitConverter.ToString(hmacBytes)
                .ToLower()
                .Replace("-", ""); // byte型配列を16進数の文字列に変換
            return hmacText;
        }

        static readonly UTF8Encoding encoding = new(false);
    }
}