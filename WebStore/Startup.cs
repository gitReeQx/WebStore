using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Services;
using WebStore.Infrastructure.Services.InCookies;
using WebStore.Infrastructure.Services.InSQL;

namespace WebStore
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) => _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(_configuration.GetConnectionString("Default")));

            services.AddTransient<WebStoreDbInitializer>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM1234567890";

                opt.Lockout.AllowedForNewUsers = false;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore.DB";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
            services.AddTransient<IProductData, SQLProductData>();
            services.AddScoped<ICartService, InCookiesCartService>();

            services
                .AddControllersWithViews(opt =>
                {
                    //opt.Conventions.Add(new WebStoreControllerConvention());
                })
                .AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDbInitializer db)
        {
            db.Ititialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/greeting", async context => await context.Response.WriteAsync(_configuration["greeting"]));

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            
                endpoints.MapControllerRoute(
                    name:"Default",
                    pattern:"{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
