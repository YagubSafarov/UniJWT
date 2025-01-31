using Newtonsoft.Json;
using System.Collections.Generic;

namespace UniJWT
{
    public class JwtHandler
    {
        private static Dictionary<string, string> DefaultHeaders = new() { { "alg", "HS256" }, { "typ", "JWT" } };

        private Dictionary<string, IAlgorithm> _algorithms = new();
        public static JwtHandler Create()
        {
            JwtHandler jwt = new JwtHandler();
            HS256Algorithm hS256Algorithm = new HS256Algorithm();
            jwt.AddAlgorithm(hS256Algorithm);
            return jwt;
        }

        private JwtHandler()
        {

        }

        public void AddAlgorithm(IAlgorithm algorithm)
        {
            _algorithms.Add(algorithm.AlgorithmName, algorithm);
        }

        public Dictionary<string, object> ValidateTokenSignatureAndGetPayload(string jwt, string secret)
        {
            string[] parts = jwt.Split('.');
            if (parts.Length != 3)
            {
                throw new InvalidTokenException();
            }

            string headerBase64 = parts[0];
            string payloadBase64 = parts[1];
            string signatureBase64 = parts[2];

            try
            {
                Dictionary<string, string> headers = JsonConvert.DeserializeObject<Dictionary<string, string>>(URLUtility.Base64UrlDecode(headerBase64));

                if (!headers.ContainsKey("alg"))
                    throw new InvalidTokenException();

                string algorithmName = headers["alg"];

                if (!_algorithms.ContainsKey(algorithmName))
                {
                    throw new UnsupportedAlgorithmException();
                }

                if (headers.ContainsKey("typ") && headers["typ"] != "JWT")
                {
                    throw new UnsupportedTypeException();
                }

                var algorithm = _algorithms[algorithmName];

                string computedSignature = algorithm.CreateSignature($"{headerBase64}.{payloadBase64}", secret);

                if (signatureBase64 != computedSignature)
                {
                    throw new InvalidSignatureException();
                }

                string payloadJson = URLUtility.Base64UrlDecode(payloadBase64);
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadJson);
            }
            catch
            {
                throw new InvalidTokenException();
            }
        }

        public string GenerateToken(Dictionary<string, object> payload, string secret, Dictionary<string, string> headers = null, string algorithmName = "HS256")
        {
            if (headers == null)
            {
                headers = DefaultHeaders;
            }
            else
            {
                if (headers.ContainsKey("alg") && headers["alg"] != algorithmName)
                {
                    headers["alg"] = algorithmName;
                }
                else if (!headers.ContainsKey("alg"))
                {
                    headers.Add("alg", algorithmName);
                }


                if (headers.ContainsKey("typ") && headers["typ"] != "JWT")
                {
                    throw new UnsupportedTypeException();
                }
                else if (!headers.ContainsKey("typ"))
                {
                    headers.Add("typ", "JWT");
                }

            }

            string headersBase64 = URLUtility.Base64UrlEncode(JsonConvert.SerializeObject(headers));
            string payloadBase64 = URLUtility.Base64UrlEncode(JsonConvert.SerializeObject(payload));

            if (!_algorithms.ContainsKey(algorithmName))
            {
                throw new UnsupportedAlgorithmException();
            }

            var algorithm = _algorithms[algorithmName];

            string signature = algorithm.CreateSignature($"{headersBase64}.{payloadBase64}", secret);

            return $"{headersBase64}.{payloadBase64}.{signature}";
        }
    }
}
