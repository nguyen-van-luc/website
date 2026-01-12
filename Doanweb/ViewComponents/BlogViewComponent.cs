using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Doanweb.Models;

namespace Harmic1.BlogViewComponents
{
    public class BlogViewComponent : ViewComponent
    {
        private readonly ChothueContext _context;
        public BlogViewComponent(ChothueContext context)
        {
            _context = context;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _context.TbBlogs
                .Where(m => (bool)m.IsActive);
            return await Task.FromResult<IViewComponentResult>
            (View(items.OrderByDescending(m => m.BlogId).ToList()));
        }
    }
}
