using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {

        private readonly BlogDbContext _dbContext;

        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Posts.Any()) return;

            var authors = AddAuthors();
            var caregories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, caregories, tags);
        }


        private IList<Author> AddAuthors()
        {
            var authors = new List<Author>()
        {
                new()
                {
                    FullName = "Jason Mouth",
                    UrlSlug = "jason-mouth",
                    Email = "json@gmail.com",
                    JoinedDate = new DateTime(2022, 10, 21)
                },
                new()
                {
                    FullName = "Jessica Wonder",
                    UrlSlug = "jessica-wonder",
                    Email = "jessica665@motip.com",
                    JoinedDate = new DateTime(2020, 4, 19)
                },
                new()
                {
                    FullName = "Kathy Smith",
                    UrlSlug = "Kathy-Smith",
                    Email = "Kathy.Smith@iwordl.com",
                    JoinedDate = new DateTime(2018, 1, 20)
                },
                new()
                {
                    FullName = "Pham Son",
                    UrlSlug = "Pham-Son",
                    Email = "phamson@gmail.com",
                    JoinedDate = new DateTime(2019, 2, 15)
                },
                new()
                {
                    FullName = "Pham Ha",
                    UrlSlug = "Pham-Ha",
                    Email = "phamha@gmail.com",
                    JoinedDate = new DateTime(2017, 3, 17)
                }
        };

            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();

            return authors;
        }

        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
        {
                new() {Name = ".NET Core", Description = "NET Core", UrlSlug = "Net core"},
                new() {Name = "Architeture", Description = "Architeture", UrlSlug = "Architeture"},
                new() {Name = "Messaging", Description = "Messaging", UrlSlug = "Messaging"},
                new() {Name = "OOP", Description = "object-oriented-program", UrlSlug = "Object-oriented-program"},
                new() {Name = "Design Pattern", Description = "Design Pattern", UrlSlug = "Design pattern"},
                new() {Name = "C#", Description = "là một ngôn ngữ lập trình hướng đối tượng đa năng, mạnh mẽ được phát triển bởi Microsoft, C# là phần khởi đầu cho kế hoạch .NET của họ", UrlSlug = "#\","},
                new() {Name = "HTML", Description = "là ngôn ngữ đánh dấu tiêu chuẩn cho các trang Web.\r\n\r\nVới HTML, bạn có thể tạo Trang web của riêng mình.", UrlSlug = "HTML"},
                new() {Name = "CSS", Description = "là ngôn ngữ chúng tôi sử dụng để tạo kiểu cho tài liệu HTML.\r\n\r\nCSS mô tả cách hiển thị các phần tử HTML.", UrlSlug = "CSS"},
                new() {Name = "JavaScript", Description = "là ngôn ngữ lập trình phổ biến nhất thế giới.\r\n\r\nJavaScript là ngôn ngữ lập trình của Web.\r\n\r\nJavaScript rất dễ học.", UrlSlug = "JS"},
                new() {Name = "ReactJS", Description = "một thư viện JavaScript front-end mã nguồn mở và miễn phí[2] để xây dựng giao diện người dùng dựa trên các thành phần UI riêng lẻ", UrlSlug = "ReactJS"},
        };

            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();

            return categories;
        }

        private IList<Tag> AddTags()
        {
            var tags = new List<Tag>()
        {
            new() {Name = "Google", Description = "Google is an internet search ", UrlSlug = "google"},
            new() {Name = "ASP.NET MVC", Description = "Framework", UrlSlug = "asp net mvc"},
            new() {Name = "Razor Page", Description = "This is the first tutorial", UrlSlug = "razor page"},
            new() {Name = "Blazor", Description = "Blazor is a  web framework", UrlSlug = "blazor"},
            new() {Name = "Deep Learning", Description = "Deep learning  learning", UrlSlug = "deep learning"},
            new() {Name = "Neural Network", Description = "A neural network is a ", UrlSlug = "neural network"},
            new() {Name = "ReactJS", Description = "Thư viện JavaScript để xây dựng giao diện người dùng dựa trên các thành phần UI riêng lẻ", UrlSlug = "reactjs.com"},
            new() {Name = "Bootstrap", Description = "Framework CSS phổ biến để xây dựng giao diện web tương thích trên nhiều thiết bị khác nhau", UrlSlug = "google"},
            new() {Name = "VueJS", Description = "VueJS", UrlSlug = "VueJS"},
            new() {Name = "Blockchain", Description = "Một công nghệ lưu trữ dữ liệu phân tán và bảo mật. Nó được sử dụng rộng rãi trong ngành tài chính và giao dịch tiền điện tử", UrlSlug = "Blockchain"},
            new() {Name = "AI", Description = "(trí tuệ nhân tạo): Là khoa học về máy tính và robot, tập trung nghiên cứu và phát triển các công nghệ để máy tính có thể học tư duy như con người", UrlSlug = " intelligence"},
            new() {Name = "IoT", Description = "(Internet of Things): Là một hệ thống kết nối và trao đổi thông tin giữa các thiết bị vật lý thông qua internet", UrlSlug = "overview.html"},
            new() {Name = "Big data", Description = "(Dữ liệu lớn): Là các dữ liệu quá lớn, phức tạp và đa dạng để xử lý bằng các công cụ truyền thống", UrlSlug = "what-is-big-data"},
            new() {Name = "Cybersecurity", Description = "(Bảo mật mạng): Là các phương pháp và công nghệ để bảo vệ thông tin và hệ thống mạng khỏi các cuộc tấn công, đánh cắp dữ liệu và virus.", UrlSlug = "cybersecurity.html"},
        };
            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();

            return tags;
        }

        private IList<Post> AddPosts(
        IList<Author> authors,
        IList<Category> categories,
        IList<Tag> tags)
        {
            var posts = new List<Post>()
        {
            new()
            {
                Title = "ASP.NET Core Dianostic Scenarios",
                ShortDescription = "David and friends has a great repository filled",
                Description = "Here's a few great DON'T and DO examples, but be sure to Star the repo and check it out for yourself!",
                Meta = "David and friends has a great repository filled with examples",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published= true,
                PostedDate= new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[1],
                Category= categories[1],
                Tags= new List<Tag>()
                {
                    tags[1],tags[4]
                }
            },
            new()
            {
                Title = "Cybersecurity",
                ShortDescription = " Là các phương pháp và công nghệ để bảo vệ thông tin và hệ th",
                Description = "ống mạng khỏi các cuộc tấn công, đánh cắp dữ liệu và virus ",
                Meta = "ống mạng khỏi các cuộc tấn công, đánh cắp dữ liệu và virush",
                UrlSlug = "tan-bossn-tich-phan",
                Published= true,
                PostedDate= new DateTime(2021, 9, 28, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 0,
                Author = authors[2],
                Category= categories[2],
                Tags= new List<Tag>()
                {
                    tags[2],tags[5],tags[7]
                }
            },
            new()
            {
                Title = "Blockchain",
                ShortDescription = " Nó được sử dụng rộng rãi trong ngành tài chính và giao dịch tiền điện tử",
                Description = "Một công nghệ lưu trữ dữ liệu phân tán và bảo mật. ",
                Meta = "phân tán và bảo mật. Nó được sử dụng rộng rãi trong ngành",
                UrlSlug = "tan-bon-tich-phan",
                Published= true,
                PostedDate= new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 11,
                Author = authors[3],
                Category= categories[3],
                Tags= new List<Tag>()
                {
                    tags[3],tags[6]
                }
            },

            new()
            {
                Title = "Hệ thống Internet của vật",
                ShortDescription = "Hệ thống Internet của vật là một công nghệ mới được phát triển để kết nối các thiết bị Internet of Things (IoT) với nhau bằng cách sử dụng các cảm biến, bộ vi điều khiển và kết nối mạng không dây",
                Description = "Hệ thống này cho phép các thiết bị IoT giao tiếp với nhau và chia sẻ thông tin mà không cần phải thông qua một thiết bị trung gian, giúp tăng tính linh hoạt và tối ưu hiệu suất của các thiết bị IoT ",
                Meta = " (Meta)Hệ thống Internet của vật sẽ đóng vai trò quan trọng trong việc xây dựng một môi trường kết nối hoàn chỉnh và thông minh trong thế giới IoT ngày càng phát triển.",
                UrlSlug = "pham-ly-thanh",
                Published= true,
                PostedDate= new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 11,
                Author = authors[3],
                Category= categories[3],
                Tags= new List<Tag>()
                {
                    tags[3],tags[6]
                }
            },
            new()
            {
                Title = "Trí tuệ nhân tạo (Artificial Intelligence - AI)",
                ShortDescription = " Trí tuệ nhân tạo là khả năng của máy tính hoặc hệ thống máy tính",
                Description = " để thực hiện các tác vụ giống như con người, bao gồm cả học và tự động hoá các quyết định. ",
                Meta = "Nó được sử dụng rộng rãi trong ngành IT",
                UrlSlug = "tam-tam-a",
                Published= true,
                PostedDate= new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 11,
                Author = authors[3],
                Category= categories[3],
                Tags= new List<Tag>()
                {
                    tags[3],tags[6]
                }
            }
        };

            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();

            return posts;
        }
    }
}
