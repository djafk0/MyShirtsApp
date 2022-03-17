namespace MyShirtsApp.Infrastructure
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;

    using static WebConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);
            SeedSizes(services);
            SeedAdministrator(services);

            return app;
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<MyShirtsAppDbContext>();

            data.Database.Migrate();
        }

        private static void SeedSizes(IServiceProvider services)
        {
            var data = services.GetRequiredService<MyShirtsAppDbContext>();

            if (data.Sizes.Any())
            {
                return;
            }

            data.Sizes.AddRange(new[]
            {
                new Size { Name = "XS" },
                new Size { Name = "S" },
                new Size { Name = "M" },
                new Size { Name = "L" },
                new Size { Name = "XL" },
                new Size { Name = "XXL" },
            });

            data.SaveChanges();
        }

        private static void SeedAdministrator(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole
                    {
                        Name = AdministratorRoleName
                    };

                    await roleManager.CreateAsync(role);

                    const string admin = "admin@msa.bg";
                    const string adminPassword = "admin123";

                    var user = new User
                    {
                        Email = admin,
                        UserName = admin,
                        FullName = "Admin"
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}
