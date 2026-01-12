using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Linq; // Cần thiết cho .OrderBy

namespace Doanweb.Utilities
{
    public class VnPayLibrary
    {
        private readonly SortedDictionary<string, string> _requestData = new();

        public void AddRequestData(string key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                _requestData[key] = value;
        }

        // VNPAY yêu cầu encode chuẩn RFC3986 để đồng nhất mã băm
        private static string UrlEncodeRfc3986(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            var encoded = WebUtility.UrlEncode(input);
            return encoded.Replace("+", "%20").Replace("%7e", "~");
        }

        public string CreateRequestUrl(string baseUrl, string vnp_HashSecret)
        {
            StringBuilder data = new StringBuilder();

            // 1. Sắp xếp và nối chuỗi dùng UrlEncodeRfc3986
            foreach (KeyValuePair<string, string> kv in _requestData.OrderBy(n => n.Key))
            {
                if (!string.IsNullOrEmpty(kv.Value))
                {
                    // Dùng hàm băm cho cả KEY và VALUE
                    data.Append(UrlEncodeRfc3986(kv.Key) + "=" + UrlEncodeRfc3986(kv.Value) + "&");
                }
            }

            string rawData = data.ToString();
            if (rawData.EndsWith("&")) rawData = rawData.Remove(rawData.Length - 1);

            // 2. Tạo URL thanh toán
            string paymentUrl = baseUrl + "?" + rawData;

            // 3. Tạo chữ ký (Gọi trực tiếp hàm trong class, xóa Utils.)
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, rawData);
            paymentUrl += "&vnp_SecureHash=" + vnp_SecureHash;

            return paymentUrl;
        }

        // Hàm băm chuẩn HMACSHA512 cho VNPAY
        public static string HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }
            return hash.ToString();
        }
    }
}