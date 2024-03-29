﻿namespace TatBlog.WebApp.Extensions
{
    public static class RouteExtensions
    {
        // Định nghĩa route template, route constraint cho các
        // endpoints kết hợp với các action trong các controller.
        public static IEndpointRouteBuilder UseBlogRoutes(this IEndpointRouteBuilder endpoints)
        {
        
            endpoints.MapControllerRoute(
                name: "post-by-category",
                pattern: "blog/category/{slug}",
                defaults: new { controller = "Blog", action = "Category" }
              );
            endpoints.MapControllerRoute(
                name: "post-by-author",
                pattern: "blog/author/{slug}",
                defaults: new { controller = "Blog", action = "Author" }
              );
            endpoints.MapControllerRoute(
                name: "post-by-tag",
                pattern: "blog/tag/{slug}",
                defaults: new { controller = "Blog", action = "Tag" }
              );


            endpoints.MapControllerRoute(
                name: "single-post",
                pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
                defaults: new { controller = "Blog", action = "Post" }
              );
            endpoints.MapControllerRoute(
		        name: "admin-area",
		        pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
		        defaults: new { area = "Admin" }

	          );

            endpoints.MapAreaControllerRoute(
                              name: "admin",
                              areaName: "admin",
                              pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                            name: "admin-area",
                            pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Blog}/{action=Index}/{id?}"
              );

            return endpoints;
        }
    }
}
