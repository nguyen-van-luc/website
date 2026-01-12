using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Data;

namespace YourProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : Controller
    {
        private readonly string _connectionString = "Server=ADMIN\\NGA;Database=Harmic;Trusted_Connection=True;";
        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest req)
        {
            string question = req.Question?.ToLower() ?? "";


            if (question.Contains("sản phẩm"))
            {
                var result = await GetProducts();
                if (result.Rows.Count > 0)
                    return Ok($"Có {result.Rows.Count} sản phẩm trong kho.");
                else
                    return Ok("Không tìm thấy sản phẩm nào trong kho.");
            }

            else if (question.Contains("giá"))
            {
                string[] parts = question.Split(' ');
                string productName = "";
                foreach (string word in parts)
                {
                    if (word != "giá" && word != "là" && word != "bao" && word != "nhiêu" && word != "?")
                        productName = word.Trim();
                }

                if (!string.IsNullOrEmpty(productName))
                {
                    var product = await GetPriceInfo(productName);
                    if (product != null)
                    {
                        string title = product["Title"].ToString();
                        string price = product["Price"].ToString();
                        string sale = product["PriceSale"]?.ToString();

                        if (!string.IsNullOrEmpty(sale) && sale != "0")
                        {
                            return Ok($"Giá của {title} là {price}đ, hiện đang khuyến mãi còn {sale}đ 🎉");
                        }
                        else
                        {
                            return Ok($"Giá của {title} là {price}đ.");
                        }
                    }
                    else
                    {
                        return Ok($"Không tìm thấy sản phẩm '{productName}'.");
                    }
                }

                return Ok("Bạn vui lòng cho tôi biết tên sản phẩm cần tra giá nhé!");
            }
            else if (question.Contains("tính tiền"))
            {

                var parts = question.Replace("tính tiền", "").Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var items = new List<(string name, int qty)>();
                int qty = 0;
                for (int i = 0; i < parts.Length; i++)
                {
                    if (int.TryParse(parts[i], out qty))
                    {
                        if (i + 1 < parts.Length)
                        {
                            string name = parts[i + 1];
                            items.Add((name, qty));
                            i++;
                        }
                    }
                }

                if (items.Count == 0)
                    return Ok("Bạn vui lòng nhập số lượng và tên sản phẩm, ví dụ: 'tính tiền 2 chuối 3 ổi'.");

                int total = 0;
                string detail = "";
                foreach (var item in items)
                {
                    var product = await GetPriceInfo(item.name);
                    if (product != null)
                    {
                        int price = int.Parse(product["PriceSale"]?.ToString() ?? product["Price"].ToString());
                        int itemTotal = price * item.qty;
                        total += itemTotal;
                        detail += $"{item.qty} {product["Title"]}: {itemTotal}đ\n";
                    }
                    else
                    {
                        detail += $"Không tìm thấy sản phẩm '{item.name}'.\n";
                    }
                }
                return Ok($"Chi tiết:\n{detail}\nTổng tiền: {total}đ");
            }

            else if (question.Contains("giờ mở cửa") || question.Contains("giờ làm việc"))
            {
                return Ok("Shop mở cửa từ 8h00 đến 21h00 mỗi ngày, kể cả cuối tuần.");
            }

            else if (question.Contains("địa chỉ") || question.Contains("ở đâu"))
            {
                return Ok("Địa chỉ shop: số 182, đường Lê Duẩn, thành phố Vinh, tỉnh Nghệ An.");
            }

            else if (question.Contains("giao hàng") || question.Contains("vận chuyển"))
            {
                return Ok("Shop giao hàng toàn quốc, miễn phí với đơn từ 200.000đ.");
            }

            else if (question.Contains("liên hệ") || question.Contains("số điện thoại"))
            {
                return Ok("Bạn có thể liên hệ shop qua số điện thoại 3965410 hoặc email info@example.com.");
            }
            else if (question.Contains("oke") || question.Contains("cảm ơn"))
            {
                return Ok("Vâng. Cảm ơn bạn đã sử dung dịch vụ bên shop. Bạn có gì thắc mắc thì có thể hỏi thêm ạ!");
            }


            else
            {
                return Ok("Xin lỗi, tôi chưa hiểu câu hỏi này. Bạn vui lòng hỏi lại hoặc liên hệ trực tiếp với shop nhé!");
            }
        }
        private async Task<DataTable> GetProducts()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT TOP 5 Title, Price FROM dbo.tb_Product";
                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        private async Task<DataRow> GetPriceInfo(string keyword)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT TOP 1 Title, Price, PriceSale 
                                 FROM dbo.tb_Product 
                                 WHERE Title LIKE @keyword";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                await conn.OpenAsync();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }
    }

    public class ChatRequest
    {
        public string Question { get; set; }
    }
}
