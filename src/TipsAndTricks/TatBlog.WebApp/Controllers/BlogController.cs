using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            [FromQuery(Name = "k")] string keyword = "",
            [FromQuery(Name = "ps")] int pageSize = 3
            )
        {
            //tạo đối tượng chứ các điều kiện truy vấn
            var postQuery = new PostQuery()
            {
                // chỉ lấy những bài viết có trang thái published
                PublishedOnly = true,
                //tìm kiếm theo từ khoá 
                Keyword = keyword
            };
            // truy vấn các bài viết theo điều kiện đã tạo
            var postsList = await _blogRepository.GetPagedPostAsync(
                postQuery, pageNumber, pageSize);
            // lưu lại điều kiện truy vấn để hiện thị trong view
            ViewBag.PostQuery = postQuery;

            return View(postsList);
        }


        //hiện thị category
        public async Task<IActionResult> Category( string slug = "")
        {
            if (slug == null) return NotFound();

            var postQuery = new PostQuery
            {
                CategorySlug = slug
            };

            var posts = await _blogRepository.GetPostByQueryAsync(postQuery);

            return View(posts);
        }

        // hiện thi các tác giả 
        public async Task<IActionResult> Author(string slug = "")
        {
            if (slug == null) return NotFound();

            var postQuery = new PostQuery
            {
                AuthorSlug = slug
            };

            var posts = await _blogRepository.GetPostByQueryAsync(postQuery);

            return View(posts);
        }

        // khi bấm vô sẽ xuất hiện các những thẻ có trong bài viết
        public async Task<IActionResult> Tag(string slug = "")
        {
            if (slug == null) return NotFound();

            var postQuery = new PostQuery
            {
                TagSlug = slug
            };

            var posts = await _blogRepository.GetPostByQueryAsync(postQuery);

            var tag = await _blogRepository.GetTagBySlugAsync(slug);
            ViewData["Tag"] = tag;

            return View(posts);
        }

        // để hiển thị chi tiết một bài viết khi người dùng nhấn vào nút Xem chi tiết
        public async Task<IActionResult> Post(int year = 2023, int month = 1,int day = 1, string slug = "")
        {
            if (slug == null) return NotFound();

            var post = await _blogRepository.GetPostAsync(year, month, day, slug);

            if (post == null) return Content("Không tìm thấy bài viết !!!");

            if (!post.Published)
            {
                ModelState.AddModelError("not access", "Bài viết này lỗi kko truy cập đc");
                return View();
            }
            else
            {
                await _blogRepository.IncreaseViewCountAsync(post.Id);
            }

            return View(post);
        }

        // hiện thị danh sách bài viết được đăng trong tháng và năm đã chọn(do người
        // dùng click chuột vào các tháng trong view component Archives ở bài tập 3).
        public async Task<IActionResult> Archives(int year, int month)
        {
            PostQuery query = new PostQuery
            {
                Year = year,
                Month = month
            };

            var posts = await _blogRepository.GetPostByQueryAsync(query);

            ViewData["PostQuery"] = query;

            return View(posts);
        }
    }
}
