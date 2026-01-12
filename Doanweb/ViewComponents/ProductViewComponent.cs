using Microsoft.AspNetCore.Mvc;
using Doanweb.Models;
using Microsoft.EntityFrameworkCore;

namespace Doanweb.ViewComponents
{
    public class ProductViewComponent : ViewComponent
    {
        private readonly ChothueContext _context;
        public ProductViewComponent(ChothueContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = _context.TbProducts.Include( m => m.CategoryProduct)
                .Where(m => (bool)m.IsActive);
             
            return await Task.FromResult<IViewComponentResult>(View(items.OrderByDescending(m => m.ProductId).ToList()));
        }

    }
}
