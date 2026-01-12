using Doanweb.Models;
using Microsoft.AspNetCore.Mvc;

namespace Doanweb.ViewComponents
{
    public class MenuTopViewComponent : ViewComponent
    {
        private readonly ChothueContext _context;

        public MenuTopViewComponent(ChothueContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var item = _context.TbMenus.Where(static m => (bool)m.IsActive)
               .OrderBy(m => m.Position).ToList();
            return await Task.FromResult<IViewComponentResult>(View(item));
        }

    }
}
