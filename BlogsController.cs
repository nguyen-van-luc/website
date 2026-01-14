using Doanweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doanweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly ChothueContext _context;

        public BlogsController(ChothueContext context)
        {
            _context = context;
        }

        // ===== LIST =====
        public async Task<IActionResult> Index()
        {
            var blogs = await _context.TbBlogs
                .Include(x => x.Category)
                .Include(x => x.Account)
                .OrderByDescending(x => x.CreatedDate)
                .ToListAsync();

            return View(blogs);
        }

        // ===== CREATE (GET) =====
        public IActionResult Create()
        {
            ViewBag.Categories = _context.TbCategories.ToList();
            ViewBag.Accounts = _context.TbAccounts.ToList();
            return View();
        }

        // ===== CREATE (POST) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TbBlog model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _context.TbCategories.ToList();
                ViewBag.Accounts = _context.TbAccounts.ToList();
                return View(model);
            }

            model.CreatedDate = DateTime.Now;
            model.IsActive ??= true;

            _context.TbBlogs.Add(model);
            await _context.SaveChangesAsync();

            TempData["ok"] = "Thêm blog thành công";
            return RedirectToAction(nameof(Index));
        }

        // ===== EDIT (GET) =====
        public async Task<IActionResult> Edit(int id)
        {
            var blog = await _context.TbBlogs.FindAsync(id);
            if (blog == null) return NotFound();

            ViewBag.Categories = _context.TbCategories.ToList();
            ViewBag.Accounts = _context.TbAccounts.ToList();
            return View(blog);
        }

        // ===== EDIT (POST) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TbBlog model)
        {
            if (id != model.BlogId) return BadRequest();

            var blog = await _context.TbBlogs.FindAsync(id);
            if (blog == null) return NotFound();

            blog.Title = model.Title;
            blog.Alias = model.Alias;
            blog.CategoryId = model.CategoryId;
            blog.Description = model.Description;
            blog.Detail = model.Detail;
            blog.Image = model.Image;
            blog.SeoTitle = model.SeoTitle;
            blog.SeoDescription = model.SeoDescription;
            blog.SeoKeywords = model.SeoKeywords;
            blog.AccountId = model.AccountId;
            blog.IsActive = model.IsActive;
            blog.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["ok"] = "Cập nhật blog thành công";
            return RedirectToAction(nameof(Index));
        }

        // ===== DELETE =====
        public async Task<IActionResult> Delete(int id)
        {
            var blog = await _context.TbBlogs
                .Include(x => x.TbBlogComments)
                .FirstOrDefaultAsync(x => x.BlogId == id);

            if (blog == null) return NotFound();

            // Xóa comment trước
            _context.TbBlogComments.RemoveRange(blog.TbBlogComments);
            _context.TbBlogs.Remove(blog);

            await _context.SaveChangesAsync();

            TempData["ok"] = "Đã xóa blog";
            return RedirectToAction(nameof(Index));
        }
    }
}
