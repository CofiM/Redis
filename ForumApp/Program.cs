using ForumApp.Cache.Interfaces;
using ForumApp.Core.Interfaces;
using ForumApp.Core.Interfaces.CacheServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ForumApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();

            using (var scope = webHost.Services.CreateScope())
            {
                var commentPostCacheService = scope.ServiceProvider.GetService<ICommentPostCacheService>();
                var communitiesCacheService = scope.ServiceProvider.GetService<ICommunityService>();
                var userInfoCacheService = scope.ServiceProvider.GetService<IUserInfoCacheService>();

                commentPostCacheService.BuildOrRefreshCache();
                communitiesCacheService.BuildOrRefreshCache();
                userInfoCacheService.BuildOrRefreshCache();
            }

            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
