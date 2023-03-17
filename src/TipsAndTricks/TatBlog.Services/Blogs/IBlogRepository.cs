using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public interface IBlogRepository
    {
        Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default);

        Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default);

        Task<bool> IsPostSlugExistedAsync(
            int postId,
            string slug,
            CancellationToken cancellationToken = default);

        Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default);
        Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default);
        Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default);


        //======================phần C==========================

        // Lấy tag theo (Slug)
        //public Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);
        //    Task<Tag> GetTagBySlugAsync(object slug);
        //}

        //======================Lab 02 -B==========================

        Task<IPagedList<Post>> GetPagedPostAsync(
            PostQuery postQuery,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
        Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query,
            int pageNumber = 1, 
            int pageSize = 10, 
            CancellationToken cancellationToken = default);

        // Tìm một thẻ (Tag) theo tên định danh (slug) 
        Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

        // Tìm bài viết có tên định danh là 'slug
        // và được đăng vào tháng 'month' năm 'year'
        Task<Post> GetPostAsync(int year, int month, int day, string slug, CancellationToken cancellationToken = default);


        // d. Lấy và phân trang danh sách tác giả kèm theo số lượng bài viết của tác giả
        // đó.Kết quả trả về kiểu IPagedList<AuthorItem>
        Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default);

        // l. Tìm một bài viết theo mã số. 
        Task<Post> GetPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default);

        // m. Thêm hay cập nhật một bài viết. 
        Task AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);
        // n. Chuyển đổi trạng thái Published của bài viết. 
        Task ChangePostStatusAsync(int id, CancellationToken cancellationToken = default);
        // i. Nút Xoá bài viết ở trang Admin
        Task<bool> DeletePostByIdAsync(int? id, CancellationToken cancellationToken = default);
    }
}
