using Doanweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Doanweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly ChothueContext _context;

        public AccountController(ChothueContext context)
        {
            _context = context;
        }

        // ===== LIST =====
        public async Task<IActionResult> Index()
        {
            var accounts = await _context.TbAccounts
                .Include(x => x.Role)
                .OrderByDescending(x => x.AccountId)
                .ToListAsync();

            return View(accounts);
        }

        // ===== CREATE (GET) =====
        public IActionResult Create()
        {
            ViewBag.Roles = _context.TbRoles.ToList();
            return View();
        }

        // ===== CREATE (POST) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TbAccount model)
        {
            // Check trùng username
            if (_context.TbAccounts.Any(x => x.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Username đã tồn tại");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _context.TbRoles.ToList();
                return View(model);
            }

            // ✅ LƯU PASSWORD THUẦN (KHÔNG HASH)
            // model.Password đã được bind từ form

            model.IsActive = true;
            model.LastLogin = null;

            _context.TbAccounts.Add(model);
            await _context.SaveChangesAsync();

            TempData["ok"] = "Thêm người dùng thành công";
            return RedirectToAction(nameof(Index));
        }

        // ===== EDIT (GET) =====
        public async Task<IActionResult> Edit(int id)
        {
            var acc = await _context.TbAccounts.FindAsync(id);
            if (acc == null) return NotFound();

            ViewBag.Roles = _context.TbRoles.ToList();
            return View(acc);
        }

        // ===== EDIT (POST) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TbAccount model)
        {
            if (id != model.AccountId) return BadRequest();

            var acc = await _context.TbAccounts.FindAsync(id);
            if (acc == null) return NotFound();

            acc.FullName = model.FullName;
            acc.Phone = model.Phone;
            acc.Email = model.Email;
            acc.RoleId = model.RoleId;
            acc.IsActive = model.IsActive;

            await _context.SaveChangesAsync();

            TempData["ok"] = "Cập nhật thành công";
            return RedirectToAction(nameof(Index));
        }

        // ===== RESET PASSWORD (GET) =====
        public async Task<IActionResult> ResetPassword(int id)
        {
            var acc = await _context.TbAccounts.FindAsync(id);
            if (acc == null) return NotFound();

            return View(acc);
        }

        // ===== RESET PASSWORD (POST) =====
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(int id, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError("", "Mật khẩu không được để trống");
            }

            var acc = await _context.TbAccounts.FindAsync(id);
            if (acc == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(acc);
            }

            // ✅ LƯU PASSWORD THUẦN
            acc.Password = newPassword;

            await _context.SaveChangesAsync();

            TempData["ok"] = "Đã đặt lại mật khẩu";
            return RedirectToAction(nameof(Index));
        }

        // ===== TOGGLE ACTIVE =====
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var acc = await _context.TbAccounts.FindAsync(id);
            if (acc == null) return NotFound();

            acc.IsActive = !(acc.IsActive ?? true);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
