using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doanweb.Models;
using Doanweb.Utilities;

namespace Doanweb.Controllers
{
    [Area("Admin")]
    public class RegisterController : Controller
    {
        private readonly ChothueContext _context;

        public RegisterController(ChothueContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Message = Function._Message;
            return View(); 
        }

        [HttpPost]
        public IActionResult Index(TbAccount account)
        {
            if (account == null)
            {
                Function._Message = "Dữ liệu không hợp lệ!";
                return RedirectToAction("Index");
            }

            var check = _context.TbAccounts.Where(m => m.Username == account.Username).FirstOrDefault();
            if (check != null)
            {
                Function._Message = "Trùng tài khoản";
                return RedirectToAction("Index");
            }

            Function._Message = string.Empty;
            // Trim password to avoid whitespace issues
            string passwordInput = account.Password?.Trim() ?? "";
            account.Password = HashMD5.GetMD5(passwordInput);
            account.IsActive = true; // Đảm bảo tài khoản được kích hoạt khi đăng ký
            _context.TbAccounts.Add(account);
            _context.SaveChanges();

            return RedirectToAction("Index", "Login", new { area = "Admin" });
        }
    }
}
