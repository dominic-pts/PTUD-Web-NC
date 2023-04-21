﻿using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.Services.Extensions;

namespace TatBlog.WebApi.Endpoints;

public static class PostEndpoints
{
    public static WebApplication MapPostEndpoints(this WebApplication app)
    {
        var routeGroupBuilder = app.MapGroup("/api/posts");

        // Nested Map with defined specific route
        routeGroupBuilder.MapGet("/", GetPosts)
                 .WithName("GetPosts")
                 .Produces<ApiResponse<PaginationResult<PostDto>>>();

        routeGroupBuilder.MapGet("/featured/{limit:int}", GetFeaturedPost)
                 .WithName("GetFeaturedPost")
                 .Produces<ApiResponse<IList<PostDto>>>();

        routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPost)
                 .WithName("GetRandomPost")
                 .Produces<ApiResponse<IList<PostDto>>>();

        routeGroupBuilder.MapGet("/archives/{limit:int}", GetArchivesPost)
                 .WithName("GetArchivesPost")
                 .Produces<ApiResponse<IList<DateItem>>>();

        routeGroupBuilder.MapGet("/{id:int}", GetPostDetails)
                 .WithName("GetPostById")
                 .Produces<ApiResponse<PostDto>>();

        routeGroupBuilder.MapGet("/byslug/{slug::regex(^[a-z0-9_-]+$)}", GetPostBySlug)
                 .WithName("GetPostBySlug")
                 .Produces<ApiResponse<PostDto>>();

        routeGroupBuilder.MapPost("/", AddPost)
                 .WithName("AddNewPost")
                 .Accepts<PostEditModel>("multipart/form-data")
                 //.AddEndpointFilter<ValidatorFilter<PostEditModel>>()
                 .Produces(401)
                 .Produces<ApiResponse<PostItem>>();

        routeGroupBuilder.MapDelete("/{id:int}", DeletePost)
                 .WithName("DeletePost")
                 .Produces(401)
                 .Produces<ApiResponse<string>>();

        routeGroupBuilder.MapPost("/published/switch/{id:int}", SwitchPublished)
                 .WithName("SwitchPublished")
                 .Produces(401)
                 .Produces<ApiResponse<string>>();

        routeGroupBuilder.MapPost("/{id:int}/picture", SetPostPicture)
                 .WithName("SetPostPicture")
                 .Accepts<IFormFile>("multipart/formdata")
                 .Produces(401)
                 .Produces<string>();

        routeGroupBuilder.MapGet("/{id:int}/comments", GetCommentOfPost)
                 .WithName("GetCommentOfPost")
                 .Produces<ApiResponse<PaginationResult<Comment>>>();

        routeGroupBuilder.MapGet("/get-filter", GetFilter)
                         .WithName("GetPostFilter")
                         .Produces<ApiResponse<PostFilterModel>>();

        return app;
    }

    private static async Task<IResult> GetPosts([AsParameters] PostFilterModel model, IBlogRepository blogRepository, IMapper mapper)
    {
        var postQuery = mapper.Map<PostQuery>(model);
        var postsList = await blogRepository.GetPostByQueryAsync(postQuery, model, posts => posts.ProjectToType<PostDto>());

        var paginationResult = new PaginationResult<PostDto>(postsList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetFeaturedPost(int limit, IBlogRepository blogRepository, IMapper mapper)
    {
        var posts = await blogRepository.GetPopularArticlesAsync(limit);

        var postsDto = posts.Select(p => mapper.Map<PostDto>(p)).ToList();

        return Results.Ok(ApiResponse.Success(postsDto));
    }

    private static async Task<IResult> GetRandomPost(int limit, IBlogRepository blogRepository, IMapper mapper)
    {
        var posts = await blogRepository.GetRandomPostAsync(limit);

        var postsDto = posts.Select(p => mapper.Map<PostDto>(p)).ToList();

        return Results.Ok(ApiResponse.Success(postsDto));
    }

    private static async Task<IResult> GetArchivesPost(int limit, IBlogRepository blogRepository)
    {
        var postDate = await blogRepository.GetArchivesPostAsync(limit);

        return Results.Ok(ApiResponse.Success(postDate));
    }

    private static async Task<IResult> GetPostDetails(int id, IBlogRepository blogRepository, IMapper mapper)
    {
        var post = await blogRepository.GetCachedPostByIdAsync(id);

        return post == null ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy bài viết có mã số {id}")) : Results.Ok(ApiResponse.Success(mapper.Map<PostDto>(post)));
    }

    private static async Task<IResult> GetPostBySlug(string slug, [AsParameters] PagingModel pagingModel, IBlogRepository blogRepository)
    {
        var postQuery = new PostQuery
        {
            PostSlug = slug,
            PublishedOnly = true
        };

        var postsList = await blogRepository.GetPostByQueryAsync(postQuery, pagingModel, posts => posts.ProjectToType<PostDto>());

        var post = postsList.FirstOrDefault();

        return Results.Ok(ApiResponse.Success(post));
    }

    private static async Task<IResult> AddPost(HttpContext context, IBlogRepository blogRepository, IMapper mapper, IMediaManager mediaManager)
    {
        var model = await PostEditModel.BindAsync(context);
        var slug = model.Title.GenerateSlug();

        if (await blogRepository.IsPostSlugExistedAsync(model.Id, slug))
        {
            return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug '{slug}' đã được sử dụng cho bài viết khác"));
        }

        var post = model.Id > 0 ? await blogRepository.GetPostByIdAsync(model.Id) : null;
        if (post == null)
        {
            post = new Post() { PostedDate = DateTime.Now };
        }
        post.Title = model.Title;
        post.AuthorId = model.AuthorId;
        post.CategoryId = model.CategoryId;
        post.ShortDescription = model.ShortDescription;
        post.Description = model.Description;
        post.Meta = model.Meta; 
        post.Published = model.Published;
        post.ModifiedDate = DateTime.Now;
        post.UrlSlug = slug;

        if (model.ImageFile?.Length > 0)
        {
            string hostname = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.PathBase}/";
            string uploadedPath = await mediaManager.SaveFileAsync(model.ImageFile.OpenReadStream(), model.ImageFile.FileName, model.ImageFile.ContentType);
            if (!string.IsNullOrWhiteSpace(uploadedPath))
            {
                post.ImageUrl = hostname + uploadedPath;
            }
        }

        await blogRepository.AddOrUpdatePostAsync(post, model.GetSelectedTags());

        return Results.Ok(ApiResponse.Success(mapper.Map<PostItem>(post), HttpStatusCode.Created));
    }

    private static async Task<IResult> DeletePost(int id, IBlogRepository blogRepository)
    {
        return await blogRepository.DeletePostByIdAsync(id) ? Results.Ok(ApiResponse.Success("Post is deleted", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find post with id = {id}"));
    }

    private static async Task<IResult> SwitchPublished(int id, IBlogRepository blogRepository)
    {
        return await blogRepository.ChangePostStatusAsync(id) ? Results.Ok(ApiResponse.Success("Post is switch published", HttpStatusCode.NoContent)) : Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Could not find post with id = {id}"));
    }

    private static async Task<IResult> SetPostPicture(int id, IFormFile imageFile, IBlogRepository blogRepository, IMediaManager mediaManager)
    {
        var post = await blogRepository.GetCachedPostByIdAsync(id);
        string newImagePath = string.Empty;

        // Nếu người dùng có upload hình ảnh minh họa cho bài viết
        if (imageFile?.Length > 0)
        {
            // Thực hiện việc lưu tập tin vào thư mực uploads
            newImagePath = await mediaManager.SaveFileAsync(imageFile.OpenReadStream(), imageFile.FileName, imageFile.ContentType);

            if (string.IsNullOrWhiteSpace(newImagePath))
            {
                return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tập tin"));
            }

            // Nếu lưu thành công, xóa tập tin hình ảnh cũ (nếu có)
            await mediaManager.DeleteFileAsync(post.ImageUrl);
            post.ImageUrl = newImagePath;
        }

        return Results.Ok(ApiResponse.Success(newImagePath));
    }

    private static async Task<IResult> GetCommentOfPost(int id, [AsParameters] PagingModel pagingModel, ICommentRepository commentRepository)
    {
        var comments = await commentRepository.GetCommentByPostIdAsync(id, pagingModel.PageNumber, pagingModel.PageSize);

        var paginationResult = new PaginationResult<Comment>(comments);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetFilter(IAuthorRepository authorRepository, ICategoryRepository categoryRepository)
    {
        var model = new PostFilterModel()
        {
            AuthorList = (await authorRepository.GetAuthorsAsync())
                        .Select(a => new SelectListItem()
                        {
                            Text = a.FullName,
                            Value = a.Id.ToString()
                        }),
            CategoryList = (await categoryRepository.GetCategoriesAsync())
                            .Select(c => new SelectListItem()
                            {
                                Text = c.Name,
                                Value = c.Id.ToString()
                            })
        };

        return Results.Ok(ApiResponse.Success(model));
    }
}
