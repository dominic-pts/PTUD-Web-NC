using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
    }

   // [HttpPost]
   //public async Task<IActionResult> SubmitComment(string Name, string Email, string Message)
   // {
   //     // Xử lý bình luận ở đây, ví dụ: lưu vào cơ sở dữ liệu hoặc gửi email

   //     // Chuyển hướng người dùng đến trang cảm ơn sau khi xử lý thành công
   //     return RedirectToAction("ThankYou");
   // }
}
