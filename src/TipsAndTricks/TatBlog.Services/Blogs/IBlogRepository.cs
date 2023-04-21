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
    
        // Tìm một thẻ (Tag) theo tên định danh (slug) 
        Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);

        // Tìm bài viết có tên định danh là 'slug
        // và được đăng vào tháng 'month' năm 'year'
        Task<Post> GetPostAsync(int year, int month, int day, string slug, CancellationToken cancellationToken = default);


        // d. Lấy và phân trang danh sách tác giả kèm theo số lượng bài viết của tác giả
        // đó.Kết quả trả về kiểu IPagedList<AuthorItem>
        //Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default);

        // l. Tìm một bài viết theo mã số. 
        Task<Post> GetPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default);

        
        // m. Thêm hay cập nhật một bài viết. 
        //Task AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);
        Task<bool> AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);

        // n. Chuyển đổi trạng thái Published của bài viết. 
        Task<bool> ChangePostStatusAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> DeletePostAsync(int postId, CancellationToken cancellationToken = default);

        //Tìm Top (N) bài viết được nhiều người xem nhất trong danh sách
        Task<IList<Post>> GetPopularArticleAsync(int numPosts, CancellationToken cancellationToken = default);

        // Lấy ngẫu nhiên (N) bài viết. (N) là tham số đầu in
        Task<IList<Post>> GetPostsByQualAsync(int num, CancellationToken cancellationToken = default);

        //câu 2 lab 03
        Task<Category> FindCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> IsCategorySlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default);

        Task<IPagedList<Category>> GetCategoriesByQuery(
           CategoryQuery condition,
           int pageNumber = 1,
           int pageSize = 10,
           CancellationToken cancellationToken = default);

         // Thêm hoặc cập nhật một chuyên mục/chủ đề.
        Task<bool> AddOrEditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default);

    
        // Xóa một chuyên mục theo mã số cho trước   
        Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default);

        // Cau 3 lab03

        Task<bool> IsAuthorSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default);
        Task<IList<Author>> GetAuthorsAsync(AuthorQuery condition = null, CancellationToken cancellationToken = default);

        Task<Author> FindAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<bool> AddOrEditAuthorAsync(Author author, CancellationToken cancellationToken = default);
        Task<bool> DeleteAuthorByIdAsync(int id, CancellationToken cancellationToken = default);

        //câu3 lab03
        Task<bool> IsTagSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default);
        Task<Tag> FindTagById(int id, CancellationToken cancellationToken = default);
        Task<bool> AddOrEditTagAsync(Tag tag, CancellationToken cancellationToken = default);

        Task<IList<TagItem>> GetListTagItemAsync(TagQuery tagQuery = null, CancellationToken cancellationToken = default);

        //Xóa một thẻ theo mã cho trước
        Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default);

        //lab04

        // s. Tìm và phân trang các bài viết thỏa mãn điều kiện tìm kiếm được cho trong
        // đối tượng PostQuery(kết quả trả về kiểu IPagedList<Post>)
        Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

        Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        // t.Tương tự câu trên nhưng yêu cầu trả về kiểu IPagedList<T>.Trong đó T
        // là kiểu dữ liệu của đối tượng mới được tạo từ đối tượng Post.Hàm này có
        // thêm một đầu vào là Func<IQueryable<Post>, IQueryable<T>> mapper
        // để ánh xạ các đối tượng Post thành các đối tượng T theo yêu cầu.
        Task<IPagedList<T>> GetPostByQueryAsync<T>(PostQuery query, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default);


        // Lấy ngẫu nhiên N bài viết. N là tham số đầu vào. 
        Task<IList<Post>> GetRandomPostAsync(int limit, CancellationToken cancellationToken = default);


        Task<IList<DateItem>> GetArchivesPostAsync(int limit, CancellationToken cancellationToken = default);


        Task<Post> GetCachedPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default);

        Task<IPagedList<T>> GetPagedPostsByQueryAsync<T>(Func<IQueryable<Post>, IQueryable<T>> mapper, PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default);

        Task<bool> DeletePostByIdAsync(int? id, CancellationToken cancellationToken = default);


    }

}
