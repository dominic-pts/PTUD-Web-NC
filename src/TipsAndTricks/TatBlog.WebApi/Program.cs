using TatBlog.WebApi.Endpoints;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Mapsters;
using TatBlog.WebApi.Validations;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container
    builder.ConfigureCors()
           .ConfigureServices()
           .ConfigureNLog()
           .ConfigureSwaggerOpenApi()
           .ConfigureMapster()
           .ConfigureFluentValidation();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline
    app.SetupRequestPipeLine();

    // Configure API Endponts
    app.MapAuthorEndpoints();

    app.MapCategoryEndpoints();

    app.MapCommentEndpoints();


    app.MapPostEndpoints();

    app.MapTagEndpoints();

    app.MapSubscriberEndpoints();

    app.MapDashboardEndpoints();


    app.Run();
}