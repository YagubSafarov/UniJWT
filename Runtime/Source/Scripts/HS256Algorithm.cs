using System;
using System.Security.Cryptography;
using System.Text;

namespace UniJWT
{
    public class HS256Algorithm : IAlgorithm
    {
        public const string NAME = "HS256";

        public string AlgorithmName => NAME;

        public string CreateSignature(string data, string secret)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
        }
    }
}
