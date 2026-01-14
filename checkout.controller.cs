using Doanweb.Models;
using Doanweb.Utilities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Doanweb.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly IConfiguration _config;

        public CheckOutController(IConfiguration config)
        {
            _config = config;
        }

        // =====================
        // GET CART FROM SESSION
        // =====================
        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            return JsonConvert.DeserializeObject<List<CartItem>>(cartJson)
                   ?? new List<CartItem>();
        }

        private void ClearCart()
        {
            HttpContext.Session.Remove("Cart");
        }

        // =====================
        // GET: /Checkout
        // =====================
        [HttpGet]
        public IActionResult Index()
        {
            var cart = GetCart();
            if (!cart.Any())
                return RedirectToAction("Index", "Cart");

            return View(cart);
        }

        // =====================
        // POST: /Checkout
        // =====================
        [HttpPost]
        public IActionResult Index(string paymentMethod)
        {
            var cartItems = GetCart();
            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

            long amount = (long)Math.Round(cartItems.Sum(x => x.Total), 0);

            if (paymentMethod == "VNPAY")
            {
                var vnpay = new VnPayLibrary();

                vnpay.AddRequestData("vnp_Version", "2.1.0");
                vnpay.AddRequestData("vnp_Command", "pay");
                vnpay.AddRequestData("vnp_TmnCode", _config["VNPAY:vnp_TmnCode"]);
                vnpay.AddRequestData("vnp_Amount", (amount * 100).ToString());
                vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                vnpay.AddRequestData("vnp_CurrCode", "VND");
                vnpay.AddRequestData("vnp_Locale", "vn");
                vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1"); // FIX IP
                vnpay.AddRequestData("vnp_OrderInfo", "Thanhtoandonhang");
                vnpay.AddRequestData("vnp_OrderType", "other");
                vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());
                vnpay.AddRequestData("vnp_ReturnUrl", _config["VNPAY:vnp_Returnurl"]);


                vnpay.AddRequestData("vnp_SecureHashType", "HmacSHA512");

                string paymentUrl = vnpay.CreateRequestUrl(
                    _config["VNPAY:vnp_Url"],
                    _config["VNPAY:vnp_HashSecret"]
                );

                return Redirect(paymentUrl);
            }

            return RedirectToAction("Fail");
        }

        // =====================
        // VNPAY RETURN
        // =====================
        [HttpGet]
        public IActionResult VnpayReturn()
        {
            ClearCart();
            return View("Success");
        }

        public IActionResult Success() => View();
        public IActionResult Fail() => View();
    }
}
