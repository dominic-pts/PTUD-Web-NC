using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;

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
                new() {Name = "Core", Description = "Is a multi-purpose development platform", UrlSlug = "123"},
                new() {Name = "Architeture", Description = "The art or science of building", UrlSlug = "12231"},
                new() {Name = "Messaging", Description = "The process of sending someone", UrlSlug = "312312"},
                new() {Name = "OOP", Description = "Is a programming method", UrlSlug = "1233"},
                new() {Name = "Pattern", Description = "Is a technique in object-oriented programming", UrlSlug = "45565"},
            };

        _dbContext.AddRange(categories);
        _dbContext.SaveChanges();

        return categories;
    }

    private IList<Tag> AddTags()
    {
        var tags = new List<Tag>()
            {
                new() {Name = "IA Google", Description = "Google is an internet search engine", UrlSlug = "123"},
                new() {Name = "ABC.NET MVC", Description = "Framework is a lightweight", UrlSlug = "89869"},
                new() {Name = "node Razor Page", Description = "This is the first tutorial of a series", UrlSlug = "123123"},
                new() {Name = "Blazor zoka", Description = "Blazor zoka is a free and open-source web framework", UrlSlug = "78678"},
                new() {Name = "list Learning", Description = "list learning is a subset of machine learning", UrlSlug = "123"},
                new() {Name = "Neural sd Network", Description = "A neural ds network is a method in artificial intelligence", UrlSlug = "31231"},
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
                    Title = "6 Productivity Shortcuts on windows 10 & 11",
                    ShortDescription = "David and friends has a great repository filled",
                    Description = "Here's a few great DON'T and DO examples, but be sure to Star the repo and check it out for yourself!",
                    Meta = "David and friends has a great repository filled with examples",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published= true,
                    PostedDate= new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category= categories[0],
                    Tags= new List<Tag>()
                    {
                        tags[0]
                    }
                }
            };

        _dbContext.AddRange(posts);
        _dbContext.SaveChanges();

        return posts;
    }
}
