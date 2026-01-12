using Microsoft.AspNetCore.Mvc;
namespace Doanweb.Areas.Admin.Controllers
{
    public class FileManagerController : Controller
    {
        [Area("Admin")]
        [Route("/Admin/file-manager/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
