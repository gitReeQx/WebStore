using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB db;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly ILogger<WebStoreDbInitializer> logger;

        public WebStoreDbInitializer(
            WebStoreDB _db,
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager,
            ILogger<WebStoreDbInitializer> _Logger)
        {
            db = _db;
            userManager = UserManager;
            roleManager = RoleManager;
            logger = _Logger;
        }

        public void Ititialize()
        {
            logger.LogInformation("Инициализация БД...");

            var _db = db.Database;

            if (_db.GetPendingMigrations().Any())
            {
                logger.LogInformation("Есть непримененные миграции...");
                _db.Migrate();
                logger.LogInformation("Миграции применены успешно");
            }
            else
            {
                logger.LogInformation("Структура БД в актуальном состоянии");
            }

            try
            {
                InitializeProduct();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Ошибка при инициализации БД данными каталога товаров");
                throw;
            }

            try
            {
                InitializeIdentityAsync().Wait();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Ошибка при инициализации системы Identity");
                throw;
            }
        }

        private void InitializeProduct()
        {
            if (db.Products.Any())
            {
                logger.LogInformation("Добавление исходных данных в БД не требуется");
                return;
            }

            logger.LogInformation("Добавление секций...");

            using (db.Database.BeginTransaction())
            {
                db.Sections.AddRange(TestData.Sections);
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");
                db.SaveChanges();
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");
                db.Database.CommitTransaction();
            }

            logger.LogInformation("Добавление брендов...");

            using (db.Database.BeginTransaction())
            {
                db.Brands.AddRange(TestData.Brands);
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");
                db.SaveChanges();
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");
                db.Database.CommitTransaction();
            }

            logger.LogInformation("Добавление товаров...");

            using (db.Database.BeginTransaction())
            {
                db.Products.AddRange(TestData.Products);
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                db.SaveChanges();
                db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");
                db.Database.CommitTransaction();
            }

            logger.LogInformation("Добавление исходных данных выполнено успешно");
        }

        private async Task InitializeIdentityAsync()
        {
            async Task CheckRole(string RoleName)
            {
                if (!await roleManager.RoleExistsAsync(RoleName))
                    await roleManager.CreateAsync(new Role { Name = RoleName });
            }

            await CheckRole(Role.Administrator);
            await CheckRole(Role.User);

            if (await userManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User
                {
                    UserName = User.Administrator
                };

                var creation_result = await userManager.CreateAsync(admin, User.DefaultAdminPassword);

                if (creation_result.Succeeded)
                    await userManager.AddToRoleAsync(admin, Role.Administrator);
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"Ошибка при создании учетной записи администратора {string.Join(",", errors)}");
                }
            }
        }
    }
}
