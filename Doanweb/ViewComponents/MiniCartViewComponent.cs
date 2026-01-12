using Doanweb.Extensions;
using Doanweb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Doanweb.ViewComponents
{
    public class MiniCartViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";

        public MiniCartViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            var items = _httpContextAccessor.HttpContext?.Session.GetObjectFromJson<List<CartItem>>(CartSessionKey)
                        ?? new List<CartItem>();

            return View(items);
        }
    }
}

