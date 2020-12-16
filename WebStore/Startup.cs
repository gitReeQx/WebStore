using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Services;

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

            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
            services.AddTransient<IProductData, InMemoryProductData>();

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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/greeting", async context => await context.Response.WriteAsync(_configuration["greeting"]));
                endpoints.MapControllerRoute(
                    name:"Default",
                    pattern:"{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
