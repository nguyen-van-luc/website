using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Doanweb.Models;
using Microsoft.EntityFrameworkCore;
namespace Doanweb.Controllers
{
    public class BlogController : Controller
    {
        private readonly ChothueContext _context;

        public BlogController(ChothueContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [Route("/blog/{alias}-{id}.html")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TbBlogs == null)
            {
                return NotFound();
            }
            var blog = await _context.TbBlogs
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewBag.blogComment = _context.TbBlogComments
                .Where(i => i.BlogId == id && i.IsActive == true).ToList();
            return View(blog);
        }
    }
}
