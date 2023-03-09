

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;

var builder = WebApplication.CreateBuilder(args);
{
    //thêm các dịch vụ đc yêu cầu bởi MVC framework
    builder.Services.AddControllersWithViews();

    //đăng ký các dịch vụ với DI Container
    builder.Services.AddDbContext<BlogDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
    builder.Services.AddScoped<IDataSeeder, DataSeeder>();


}
var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Blog/Error");
        app.UseHsts();
    }
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.MapControllerRoute(name: "default", pattern: "{controller=Blog}/{action=Index}/{id?}");
}

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetService<IDataSeeder>();
    seeder.Initialize();
}


//định nghĩa route template, route constraint cho các 
//endpoints kết hợp với các action trong các controller.
app.MapControllerRoute(
    name: "posts-by-catagory",
    pattern: "blog/catagory/{slug}",
    defaults: new { Controller="Blog", action = "Category" });

app.MapControllerRoute(
    name: "posts-by-tag",
    pattern: "blog/tag/{slug}",
    defaults: new { Controller = "Blog", action = "Tag" });

app.MapControllerRoute(
    name: "single-post",
    pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
    defaults: new { Controller = "Blog", action = "Post" });
app.MapControllerRoute(
    name: "default",
    pattern: "{Controller=Blog}/{action=Index}/{id?}");



app.Run();

