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
    }
}
