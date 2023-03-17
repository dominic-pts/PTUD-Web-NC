using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class CommentsController : Controller
    {
		public async Task<IActionResult> Index()
		{
			return View();
		}
	}
}
