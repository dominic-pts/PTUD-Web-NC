using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Areas.Admin.Controllers;

public class DashboardController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}