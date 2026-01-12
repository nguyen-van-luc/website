using System.Security.Cryptography;
using System.Text;

namespace Doanweb.Utilities
{
    public class HashMD5
    {
        public static string GetMD5(string input)
        {
            using(var md5 = MD5.Create())
            {
                var data = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                var sb = new StringBuilder();
                foreach (var item in data)
                {
                    sb.Append(item.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
