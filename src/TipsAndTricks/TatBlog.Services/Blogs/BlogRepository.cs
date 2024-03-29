﻿ using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;
        private readonly ITagRepository _tagRepository;
        private readonly IMemoryCache _memoryCache;
        public BlogRepository(BlogDbContext context, IMemoryCache memoryCache, ITagRepository tagRepository)
        {
            _context = context;
            _memoryCache = memoryCache;
            _tagRepository = tagRepository;
        }

        public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postQuery = _context.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author);
            if (year > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Year == year);
            }
            if (month > 0)
            {
                postQuery = postQuery.Where(x => x.PostedDate.Month == month);
            }
            if (!string.IsNullOrWhiteSpace(slug))
            {
                postQuery = postQuery.Where(x => x.UrlSlug == slug);
            }
            return await postQuery.FirstOrDefaultAsync(cancellationToken);

        }

        public async Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsPostSlugExistedAsync(
            int postId,
            string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }
        public async Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }



        public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();
            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }
            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams,
        CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });

            return await tagQuery
                .ToPagedListAsync(pagingParams, cancellationToken);
        }



        //=====================phần C========================




        //======================Lap 02=======================

        public async Task<IPagedList<Post>> GetPagedPostAsync(
        PostQuery postQuery,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
        {
            return await FilterPosts(postQuery).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Post.PostedDate), "DESC",
                cancellationToken);
        }


        //public async Task<IPagedList<T>> GetPostByQueryAsync<T>(PostQuery query, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        //{
        //    IQueryable<T> result = mapper(FilterPosts(query));

        //    return await result.ToPagedListAsync();
        //}

        private IQueryable<Post> FilterPosts(PostQuery condition)
        {
            IQueryable<Post> posts = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags);

            if (condition.PublishedOnly)
            {
                posts = posts.Where(x => x.Published);
            }

            if (condition.NotPublished)
            {
                posts = posts.Where(x => !x.Published);
            }

            if (condition.CategoryId > 0)
            {
                posts = posts.Where(x => x.CategoryId == condition.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
            {
                posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
            }

            if (condition.AuthorId > 0)
            {
                posts = posts.Where(x => x.AuthorId == condition.AuthorId);
            }

            if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
            {
                posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
            }

            if (!string.IsNullOrWhiteSpace(condition.TagSlug))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                posts = posts.Where(x => x.Title.Contains(condition.Keyword) ||
                                         x.ShortDescription.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword) ||
                                         x.Category.Name.Contains(condition.Keyword) ||
                                         x.Author.FullName.Contains(condition.Keyword) ||
                                         x.Tags.Any(t => t.Name.Contains(condition.Keyword)));
            }

            if (condition.Year > 0)
            {
                posts = posts.Where(x => x.PostedDate.Year == condition.Year);
            }

            if (condition.Month > 0)
            {
                posts = posts.Where(x => x.PostedDate.Month == condition.Month);
            }

            if (!string.IsNullOrWhiteSpace(condition.TitleSlug))
            {
                posts = posts.Where(x => x.UrlSlug == condition.TitleSlug);
            }

            return posts;


        }

        public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                                    .Where(t => t.UrlSlug.Contains(slug))
                                    .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<Post> GetPostAsync(int year, int month, int day, string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                                                      .Include(x => x.Category)
                                                      .Include(x => x.Author)
                                                      .Include(x => x.Tags);

            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }

            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }

            if (day > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Day == day);
            }

            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }

            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<Post>> GetPopularArticleAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<Post>> GetPostsByQualAsync(int num, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .OrderBy(x => x.Id)
                .Take(num)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<TagItem>> GetListTagItemAsync(CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tagItems = _context.Set<Tag>();

            return await tagItems
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                })
            .ToListAsync(cancellationToken);
        }

        //public async Task<IList<AuthorItem>> GetAuthorsAsync(CancellationToken cancellationToken = default)
        //{
        //    var tagQuery = _context.Set<Author>()
        //                              .Select(x => new AuthorItem()
        //                              {
        //                                  Id = x.Id,
        //                                  FullName = x.FullName,
        //                                  UrlSlug = x.UrlSlug,
        //                                  ImageUrl = x.ImageUrl,
        //                                  JoinedDate = x.JoinedDate,
        //                                  Notes = x.Notes,
        //                                  PostCount = x.Posts.Count(p => p.Published)
        //                              });

        //    return await tagQuery.ToListAsync(cancellationToken);
        //}

        public async Task<Post> GetPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postQuery = _context.Set<Post>()
                                     .Include(p => p.Category)
                                     .Include(p => p.Author)
                                     .Include(p => p.Tags);

            if (published)
            {
                postQuery = postQuery.Where(x => x.Published);
            }

            return await postQuery.Where(p => p.Id.Equals(id))
                                  .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
            {
                await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
            }
            else
            {
                post.Tags = new List<Tag>();
            }

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
              .Select(x => new
              {
                  Name = x,
                  Slug = x.GenerateSlug()
              })
              .GroupBy(x => x.Slug)
              .ToDictionary(g => g.Key, g => g.First().Name);

            foreach (var kv in validTags)
            {
                if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

                var tag = await _tagRepository.GetTagBySlugAsync(kv.Key, cancellationToken) ?? new Tag()
                {
                    Name = kv.Value,
                    Description = kv.Value,
                    UrlSlug = kv.Key
                };

                post.Tags.Add(tag);
            }

            post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

            if (post.Id > 0)
                _context.Update(post);
            else
                _context.Add(post);

            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        // chuyển đổi trang thái xuất bản
        public async Task<bool> ChangePostStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var post = await _context.Posts.FindAsync(id);

            post.Published = !post.Published;

            _context.Attach(post).State = EntityState.Modified;
            return await _context.SaveChangesAsync() > 0;
        }
        // xoá  bài viết 
        //public async Task<bool> DeletePostByIdAsync(int id, CancellationToken cancellationToken = default)
        //{
        //    var post = await _context.Set<Post>().FindAsync(id);

        //    if (post is null) return false;

        //    _context.Set<Post>().Remove(post);
        //    var rowsCount = await _context.SaveChangesAsync(cancellationToken);

        //    return rowsCount > 0;
        //}

        //public async Task<bool> DeletePostByIdAsync(int id, CancellationToken cancellationToken = default)
        //{
        //    return await _context.Set<Post>()
        //        .Where(t => t.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
        //}
        public async Task<bool> DeletePostAsync(
        int postId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Set<Post>().FindAsync(postId);

            if (!post.Published) return false;

            _context.Set<Post>().Remove(post);
            var rowsCount = await _context.SaveChangesAsync(cancellationToken);

            return rowsCount > 0;
        }


        // 2 Lap03
        public async Task<Category> FindCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .FindAsync(id, cancellationToken);
        }

        public async Task<IPagedList<Category>> GetCategoriesByQuery(CategoryQuery condition, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterCategories(condition).ToPagedListAsync(
                pageNumber, pageSize,
                nameof(Category.Name), "DESC",
                cancellationToken);
        }
        private IQueryable<Category> FilterCategories(CategoryQuery condition)
        {
            IQueryable<Category> categories = _context.Set<Category>();


            if (condition.ShowOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }

            if (!string.IsNullOrWhiteSpace(condition.Keyword))
            {
                categories = categories.Where(x => x.Name.Contains(condition.Keyword) ||
                                         x.Description.Contains(condition.Keyword));
            }
            return categories;
        }

        public async Task<bool> IsCategorySlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }

        public async Task<bool> AddOrEditCategoryAsync(Category newCategory, CancellationToken cancellationToken = default)
        {
            _context.Entry(newCategory).State = newCategory.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .Where(c => c.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
        }
        // câu 3 lab03
        public async Task<bool> IsAuthorSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>().AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }
        public async Task<IList<Author>> GetAuthorsAsync(AuthorQuery condition, CancellationToken cancellationToken = default)
        {
            IQueryable<Author> authors = _context.Set<Author>();

            if (condition != null)
            {
                if (!string.IsNullOrWhiteSpace(condition.Keyword))
                {
                    authors = authors.Where(x => x.FullName.Contains(condition.Keyword) ||
                                             x.Email.Contains(condition.Keyword));
                }
            }

            return await authors.ToListAsync(cancellationToken);
        }

        public async Task<Author> FindAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>().Where(a => a.Id == id).FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<bool> AddOrEditAuthorAsync(Author author, CancellationToken cancellationToken = default)
        {
            _context.Entry(author).State = author.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> DeleteAuthorByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Author>()
                .Where(t => t.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
        }

        //Câu 4 lab03
        public async Task<bool> IsTagSlugExistedAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>().AnyAsync(x => x.Id != id && x.UrlSlug == slug, cancellationToken);
        }
        public async Task<Tag> FindTagById(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> AddOrEditTagAsync(Tag tag, CancellationToken cancellationToken = default)
        {
            _context.Entry(tag).State = tag.Id == 0 ? EntityState.Added : EntityState.Modified;
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IList<TagItem>> GetListTagItemAsync(TagQuery condition, CancellationToken cancellationToken = default)
        {
            var tagItems = _context.Set<Tag>().Select(x => new TagItem()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UrlSlug = x.UrlSlug,
                PostCount = x.Posts.Count(p => p.Published)
            });

            if (condition != null)
            {
                if (!string.IsNullOrWhiteSpace(condition.Keyword))
                {
                    tagItems = tagItems.Where(x => x.Name.Contains(condition.Keyword) ||
                                              x.Description.Contains(condition.Keyword));
                }
            }

            return await tagItems.ToListAsync(cancellationToken);
        }


        public async Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .Where(t => t.Id == id).ExecuteDeleteAsync(cancellationToken) > 0;
        }


        public async Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await FilterPosts(query).ToPagedListAsync(
                                    pageNumber,
                                    pageSize,
                                    nameof(Post.PostedDate),
                                    "DESC",
                                    cancellationToken);
        }

        public async Task<IPagedList<Post>> GetPostByQueryAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            return await FilterPosts(query).ToPagedListAsync(
                                            pagingParams,
                                            cancellationToken);
        }

        public async Task<IPagedList<T>> GetPostByQueryAsync<T>(PostQuery query, IPagingParams pagingParams, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            IQueryable<T> result = mapper(FilterPosts(query));

            return await result.ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagedPostsByQueryAsync<T>(Func<IQueryable<Post>, IQueryable<T>> mapper, PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            return await mapper(FilterPosts(query).AsNoTracking()).ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IList<Post>> GetRandomPostAsync(int limit, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>().OrderBy(p => Guid.NewGuid()).Take(limit).ToListAsync(cancellationToken);
        }

        public async Task<IList<DateItem>> GetArchivesPostAsync(int limit, CancellationToken cancellationToken = default)
        {
            var lastestMonths = await GetLatestMonthList(limit);

            return await Task.FromResult(_context.Set<Post>().AsEnumerable()
                                                                .GroupBy(p => new
                                                                {
                                                                    p.PostedDate.Month,
                                                                    p.PostedDate.Year
                                                                })
                                                                .Join(lastestMonths, d => d.Key.Month, m => m.Month,
                                                                (postDate, monthGet) => new DateItem
                                                                {
                                                                    Month = postDate.Key.Month,
                                                                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(postDate.Key.Month),
                                                                    Year = postDate.Key.Year,
                                                                    PostCount = postDate.Count()
                                                                }).ToList());
        }

        public async Task<IList<DateItem>> GetLatestMonthList(int limit)
        {
            return await Task.FromResult((from r in Enumerable.Range(1, 12) select DateTime.Now.AddMonths(limit - r))
                                .Select(x => new DateItem
                                {
                                    Month = x.Month,
                                    Year = x.Year
                                }).ToList());
        }

        public async Task<Post> GetCachedPostByIdAsync(int id, bool published = false, CancellationToken cancellationToken = default)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"post.by-id.{id}-{published}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetPostByIdAsync(id, published, cancellationToken);
                });
        }

        public async Task<bool> DeletePostByIdAsync(int? id, CancellationToken cancellationToken = default)
        {
            var post = await _context.Set<Post>().FindAsync(id);

            if (post is null) return false;

            _context.Set<Post>().Remove(post);
            var rowsCount = await _context.SaveChangesAsync(cancellationToken);

            return rowsCount > 0;
        }


        public async Task<IList<Post>> GetRandomsPostsAsync(int num, CancellationToken cancellationToken = default)
        {
            // OrderBy theo Guid random để xáo trộn List trả về
            return await _context.Set<Post>()
                .Include(a => a.Author)
                .Include(c => c.Category)
                .OrderBy(x => Guid.NewGuid())
                .Where(p => p.Published)
                .Take(num)
                .ToListAsync(cancellationToken);
        }

      

    }

}

