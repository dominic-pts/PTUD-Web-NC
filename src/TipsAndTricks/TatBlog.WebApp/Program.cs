using TatBlog.WebApp.Extensions;
using TatBlog.WebApp.Masters;
using TatBlog.WebApp.Validations;

var builder = WebApplication.CreateBuilder(args);
{
    builder.ConfigureMvc()
        .ConfigureServices()
        .ConfigureMapster()
        .ConfigureFluentValidation();
}

var app = builder.Build();
{
    app.UseRequestPipeline();
    app.UseBlogRoutes();
    app.UseDataSeeder();
}

app.Run();