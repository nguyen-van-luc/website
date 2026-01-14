using Doanweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doanweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly ChothueContext _context;

        public ProductCategoryController(ChothueContext context)
        {
            _context = context;
        }

        // ===== LIST =====
        public async Task<IActionResult> Index()
        {
            var categories = await _context.TbProductCategories
                .OrderBy(x => x.Position)
                .ToListAsync();

            return View(categories);
        }

        // ===== CREATE (GET) =====
        public IActionResult Create()
        {
            return View();
        }

        // ===== CREATE (POST) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TbProductCategory model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.CreatedDate = DateTime.Now;
            model.IsActive ??= true;

            _context.TbProductCategories.Add(model);
            await _context.SaveChangesAsync();

            TempData["ok"] = "Thêm danh mục thành công";
            return RedirectToAction(nameof(Index));
        }

        // ===== EDIT (GET) =====
        public async Task<IActionResult> Edit(int id)
        {
            var cate = await _context.TbProductCategories.FindAsync(id);
            if (cate == null) return NotFound();

            return View(cate);
        }

        // ===== EDIT (POST) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TbProductCategory model)
        {
            if (id != model.CategoryProductId) return BadRequest();

            var cate = await _context.TbProductCategories.FindAsync(id);
            if (cate == null) return NotFound();

            cate.Title = model.Title;
            cate.Alias = model.Alias;
            cate.Description = model.Description;
            cate.Icon = model.Icon;
            cate.Position = model.Position;
            cate.IsActive = model.IsActive;
            cate.ModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();

            TempData["ok"] = "Cập nhật danh mục thành công";
            return RedirectToAction(nameof(Index));
        }

        // ===== DELETE =====
        public async Task<IActionResult> Delete(int id)
        {
            var cate = await _context.TbProductCategories
                .Include(x => x.TbProducts)
                .FirstOrDefaultAsync(x => x.CategoryProductId == id);

            if (cate == null) return NotFound();

            

            _context.TbProductCategories.Remove(cate);
            await _context.SaveChangesAsync();

            TempData["ok"] = "Đã xóa danh mục";
            return RedirectToAction(nameof(Index));
        }
    }
}
