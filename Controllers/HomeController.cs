using Microsoft.AspNetCore.Mvc;

namespace ZaloOA_v2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
