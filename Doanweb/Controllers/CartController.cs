using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doanweb.Extensions;
using Doanweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doanweb.Controllers
{
    public class CartController : Controller
    {
        private readonly ChothueContext _context;
        private const string CartSessionKey = "Cart";

        public CartController(ChothueContext context)
        {
            _context = context;
        }

        // ========================
        // HIỂN THỊ GIỎ HÀNG
        // ========================
        [HttpGet]
        public IActionResult Index()
        {
            var cartItems = GetCartItems();
            return View(cartItems);
        }

        // ========================
        // SUMMARY (AJAX HEADER)
        // ========================
        [HttpGet]
        public IActionResult Summary()
        {
            var cartItems = GetCartItems();
            return Json(BuildSummary(cartItems));
        }

        // ========================
        // ADD TO CART
        // ========================
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CartRequest? request)
        {
            if (request == null || request.ProductId <= 0)
            {
                return BadRequest(new { message = "Sản phẩm không hợp lệ." });
            }

            var product = await _context.TbProducts
                .Where(p => p.ProductId == request.ProductId && p.IsActive == true)
                .Select(p => new
                {
                    p.ProductId,
                    p.Title,
                    p.Image,
                    Price = p.PriceSale.HasValue && p.PriceSale > 0
                        ? p.PriceSale
                        : p.Price
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm." });
            }

            var cartItems = GetCartItems();
            var quantityToAdd = request.Quantity > 0 ? request.Quantity : 1;

            var existingItem = cartItems.FirstOrDefault(c => c.ProductId == product.ProductId);

            if (existingItem == null)
            {
                cartItems.Add(new CartItem
                {
                    CartId = product.ProductId,
                    ProductId = product.ProductId,
                    ProductName = product.Title ?? "Sản phẩm",
                    Image = string.IsNullOrWhiteSpace(product.Image)
                        ? "no-image.png"
                        : product.Image,
                    Quantity = quantityToAdd,
                    Price = Convert.ToDecimal(product.Price ?? 0)
                });
            }
            else
            {
                existingItem.Quantity += quantityToAdd;
            }

            SaveCartItems(cartItems);
            UpdateCartTotal(cartItems);

            return Json(BuildSummary(cartItems));
        }

        // ========================
        // REMOVE ITEM
        // ========================
        [HttpPost]
        public IActionResult Remove([FromBody] CartRequest? request)
        {
            if (request == null || request.ProductId <= 0)
            {
                return BadRequest(new { message = "Sản phẩm không hợp lệ." });
            }

            var cartItems = GetCartItems();
            var item = cartItems.FirstOrDefault(c => c.ProductId == request.ProductId);

            if (item != null)
            {
                cartItems.Remove(item);
                SaveCartItems(cartItems);
                UpdateCartTotal(cartItems);
            }

            return Json(BuildSummary(cartItems));
        }

        // ========================
        // UPDATE QUANTITY
        // ========================
        [HttpPost]
        public IActionResult Update([FromBody] CartRequest? request)
        {
            if (request == null || request.ProductId <= 0 || request.Quantity <= 0)
            {
                return BadRequest(new { message = "Thông tin cập nhật không hợp lệ." });
            }

            var cartItems = GetCartItems();
            var item = cartItems.FirstOrDefault(c => c.ProductId == request.ProductId);

            if (item == null)
            {
                return NotFound(new { message = "Không tìm thấy sản phẩm trong giỏ hàng." });
            }

            item.Quantity = request.Quantity;
            SaveCartItems(cartItems);
            UpdateCartTotal(cartItems);

            return Json(BuildSummary(cartItems));
        }

        // ========================
        // SESSION HELPERS
        // ========================
        private List<CartItem> GetCartItems()
        {
            return HttpContext.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey)
                   ?? new List<CartItem>();
        }

        private void SaveCartItems(List<CartItem> items)
        {
            HttpContext.Session.SetObjectAsJson(CartSessionKey, items);
        }

        private void UpdateCartTotal(List<CartItem> items)
        {
            HttpContext.Session.SetString(
                "CartTotal",
                items.Sum(x => x.Total).ToString()
            );
        }

        // ========================
        // SUMMARY JSON
        // ========================
        private object BuildSummary(List<CartItem> items)
        {
            return new
            {
                success = true,
                cartCount = items.Sum(i => i.Quantity),
                subtotal = items.Sum(i => i.Total),
                items = items.Select(i => new
                {
                    i.ProductId,
                    i.ProductName,
                    i.Image,
                    price = i.Price,
                    quantity = i.Quantity,
                    total = i.Total
                })
            };
        }

        // ========================
        // REQUEST MODEL
        // ========================
        public class CartRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; } = 1;
        }
    }
}
