using Doanweb.Models;
using Microsoft.AspNetCore.Mvc;
using Doanweb.Models;
using System;

namespace Doanweb.Controllers
{
    public class ContactController : Controller
    {
        private readonly ChothueContext _context;

        public ContactController(ChothueContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("lien-he")]
        public IActionResult LienHe(string name, string phone, string email, string message)
        {
            var contact = new TbContact
            {
                Name = name,
                Phone = phone,
                Email = email,
                Message = message,
                CreatedDate = DateTime.Now
            };

            _context.TbContacts.Add(contact);
            _context.SaveChanges();

            return Json(new { success = true });
        }


    }
}
