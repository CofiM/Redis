using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Filters;
using ForumApp.Core.Interfaces;
using ForumApp.Services.Implementations;
using ForumApp.Core.Context;
using ForumApp.Cache;
using StackExchange.Redis;
using ForumApp.Cache.Implementation;
using ForumApp.Core.Interfaces.CacheServices;
using ForumApp.Cache.Interfaces;

namespace ForumApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ForumContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("ForumCS"));
            });

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("RedisCache");
            });

            var multiplexer = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("RedisCache"));
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);


            services.AddCors(options =>
            {
                options.AddPolicy("CORS", builder =>
                {
                    builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
            services.AddControllersWithViews();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Baze", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "Des",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                            ValidateIssuer = false,
                            ValidateLifetime = false,
                            ValidateAudience = false
                        };
                    });

            services.AddTransient<IPostLikeService, PostLikeService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<ICommunityService, CommunityService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITopicService, TopicService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<ICommentLikeOrDislikeService, CommentLikeOrDislikeService>();
            services.AddTransient<ICacheProvider, CacheProvider>();
            services.AddTransient<IFollowedCommunityServices, FollowedCommunityService>();
            services.AddTransient<ICommentPostCacheService, CommentPostCacheService>();
            services.AddTransient<IUserInfoCacheService, UserInfoCacheService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c => {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ForumApp v1");
                        c.RoutePrefix = string.Empty;
                    });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("CORS");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
