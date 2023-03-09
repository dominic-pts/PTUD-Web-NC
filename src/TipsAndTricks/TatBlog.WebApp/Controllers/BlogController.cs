using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }


        

        public IActionResult About()
        => View();

        public IActionResult Contact()
        => View();

        public IActionResult Rss()
        => Content("Nội dung sẽ được cập nhật");



       

        //Action này sử lý Http request đến trang chủ của
        //ứng dụng web hoặc tìm kiếm bài viết theo từ khoá
        public async Task<IActionResult> Index
            (
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "s")] string keyword = null,
            [FromQuery(Name = "ps")] int pageSize = 10
            )
        {
            //tạo đối tượng chứ các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // chỉ lấy những bài viết có trang thái published
                PublishedOnly = true,
                //tìm kiếm theo từ khoá 
                KeyWord = keyword
            };
            // truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository.GetPagedPostAsync(
                postQuery, pageNumber, pageSize);
            // lưu lại điều kiện truy vấn để hiện thị trong view
            ViewBag.PostQuery = postQuery;

            return View(postsList);
        }



    }
}
