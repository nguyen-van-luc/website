using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doanweb.Models;

namespace Doanweb.Controllers
{
    public class ProductController : Controller
    {
        private readonly ChothueContext _context;

        public ProductController(ChothueContext context)
        {
            _context = context;
        }

        // ================== CHI TIẾT SẢN PHẨM ==================
        [Route("/product/{alias}-{id}.html")]
        public async Task<IActionResult> Details(string alias, int id)
        {
            var product = await _context.TbProducts
                .Include(p => p.CategoryProduct)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            ViewBag.productReview = _context.TbProductReviews
                .Where(r => r.ProductId == id && r.IsActive == true)
                .OrderByDescending(r => r.CreatedDate)
                .ToList();

            ViewBag.productRelated = _context.TbProducts
                .Where(p => p.ProductId != id && p.CategoryProductId == product.CategoryProductId)
                .Take(6)
                .ToList();

            return View(product);
        }

        // ================== THÊM ĐÁNH GIÁ ==================
        [HttpPost]
        public IActionResult AddReview(
     int ProductId,
     string Alias,
     string Name,
     string Email,
     string Phone,
     int Star,
     string Detail
 )
        {
            var review = new TbProductReview
            {
                ProductId = ProductId,
                Name = Name,
                Email = Email,
                Phone = Phone,
                Star = Star,
                Detail = Detail,
                CreatedDate = DateTime.Now,
                IsActive = true
            };

            _context.TbProductReviews.Add(review);
            _context.SaveChanges();

            // ✅ redirect đúng route
            return Redirect($"/product/{Alias}-{ProductId}.html#review");
        }

    }
}
