using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doanweb.Models;
using Doanweb.Utilities;

namespace Doanweb.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private readonly ChothueContext _context;

        public LoginController(ChothueContext context)
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
                return NotFound();
            }
            string username = account.Username?.Trim();
            string passwordInput = account.Password?.Trim();

         
            string password = HashMD5.GetMD5(passwordInput);

            var check = _context.TbAccounts
                .FirstOrDefault(m => m.Username == username && m.Password == password && m.IsActive == true);

            if (check == null)
            {
             
                check = _context.TbAccounts
                    .FirstOrDefault(m => m.Username == username && m.Password == passwordInput && m.IsActive == true);
            }

            if (check == null)
            {
                Function._Message = "Invalid Username or Password";
                return RedirectToAction("Index", "Login", new { area = "Admin" });
            }

            Function._Message = string.Empty;
            Function._AccountId = check.AccountId;
            Function._UserName = check.Username;
            Function._RoleId = check.RoleId;

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}
