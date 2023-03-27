using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
