using Doanweb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Doanweb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ChothueContext _conteext;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ChothueContext context, ILogger<HomeController> logger)
        {
            _conteext = context;
            _logger = logger;
        }
        [Route("")]
        [Route("trang-chu")]


        public IActionResult Index()
        {
            ViewBag.productCategories = _conteext.TbProductCategories.ToList();
            ViewBag.productNew = _conteext.TbProducts.Where(m => (bool)m.IsNew).ToList();
            return View();
        }

        [Route("lien-he")]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("gioi-thieu")]
        public IActionResult About()
        {
            return View();
        }
        [Route("san-pham")]
        public IActionResult Product()
        {
            return View();
        }
        [Route("tin-tuc")]
        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
