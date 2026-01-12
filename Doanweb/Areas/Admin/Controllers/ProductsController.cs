using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Doanweb.Models;

namespace Doanweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ChothueContext _context;

        public ProductsController(ChothueContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var ChothueContext = _context.TbProducts.Include(t => t.CategoryProduct);
            return View(await ChothueContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts
                .Include(t => t.CategoryProduct)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (tbProduct == null)
            {
                return NotFound();
            }

            return View(tbProduct);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryProductId"] = new SelectList(_context.TbProductCategories, "CategoryProductId", "Title");
            return View();
        }
        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Title,Alias,CategoryProductId,Description,Detail,Image,Price,PriceSale,Quantity,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsNew,IsBestSeller,UnitInStock,IsActive,Star")] TbProduct tbProduct)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra CategoryProductId có tồn tại trong bảng Category trước khi lưu
                var categoryExists = await _context.TbProductCategories
                    .AnyAsync(c => c.CategoryProductId == tbProduct.CategoryProductId);

                if (!categoryExists)
                {
                    ModelState.AddModelError("CategoryProductId", "Danh mục sản phẩm không tồn tại.");
                    ViewData["CategoryProductId"] = new SelectList(_context.TbProductCategories, "CategoryProductId", "Title", tbProduct.CategoryProductId);
                    return View(tbProduct);
                }

                tbProduct.Alias = Doanweb.Utilities.Function.TitleSlugGenerrationAlias(tbProduct.Title);
                _context.Add(tbProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryProductId"] = new SelectList(_context.TbProductCategories, "CategoryProductId", "Title", tbProduct.CategoryProductId);
            return View(tbProduct);
        }

        // GET: Admin/Products/Edit/5
        public IActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var tbProduct = _context.TbProducts
                .Include(p => p.CategoryProduct)
                .FirstOrDefault(p => p.ProductId == Id);

            if (tbProduct == null)
            {
                return NotFound();
            }

            ViewData["CategoryProductId"] = new SelectList(_context.TbProductCategories, "CategoryProductId", "Title", tbProduct.CategoryProductId);
            return View(tbProduct);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Title,Alias,CategoryProductId,Description,Detail,Image,Price,PriceSale,Quantity,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,IsNew,IsBestSeller,UnitInStock,IsActive,Star")] TbProduct tbProduct)
        {
            if (id != tbProduct.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tbProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TbProductExists(tbProduct.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryProductId"] = new SelectList(_context.TbProductCategories, "CategoryProductId", "CategoryProductId", tbProduct.CategoryProductId);
            return View(tbProduct);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbProduct = await _context.TbProducts
                .Include(t => t.CategoryProduct)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (tbProduct == null)
            {
                return NotFound();
            }

            return View(tbProduct);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tbProduct = await _context.TbProducts.FindAsync(id);
            if (tbProduct != null)
            {
                _context.TbProducts.Remove(tbProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TbProductExists(int id)
        {
            return _context.TbProducts.Any(e => e.ProductId == id);
        }
    }
}
