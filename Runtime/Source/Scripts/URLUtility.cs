using System;
using System.Text;

namespace UniJWT
{
    public class URLUtility
    {
        public static string Base64UrlEncode(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public static string Base64UrlDecode(string input)
        {
            byte[] data = Convert.FromBase64String(input);
            return Encoding.UTF8.GetString(data);
        }

    }
}
