namespace MyShirtsApp.Infrastructure
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;
    using Newtonsoft.Json;
    using AutoMapper;

    using static Areas.Admin.AdminConstants;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);
            SeedAdministrator(services);
            SeedShirtsAndUsers(services);
            SeedSizes(services);
            SeedShirtSizes(services);

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

        private static void SeedShirtSizes(IServiceProvider services)
        {
            var data = services.GetRequiredService<MyShirtsAppDbContext>();

            if (data.ShirtSizes.Any())
            {
                return;
            }

            var shirtSizeJsonAsString = File.ReadAllText("./wwwroot/importShirtSizes.json");

            var shirtSizes = JsonConvert.DeserializeObject<IEnumerable<ImportShirtSizeModel>>(shirtSizeJsonAsString);

            var mapperConfiguration = new MapperConfiguration(cfg =>
                cfg.AddProfile<MappingProfile>());

            var mapper = new Mapper(mapperConfiguration);

            var mappedShirtSizes = mapper.Map<IEnumerable<ShirtSize>>(shirtSizes);

            data.ShirtSizes.AddRange(mappedShirtSizes);

            data.SaveChanges();
        }

        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

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
                    const string adminPassword = "msa123";

                    var user = new User
                    {
                        Email = admin,
                        UserName = admin,
                        CompanyName = "Admin"
                    };

                    await userManager.CreateAsync(user, adminPassword);

                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }

        private static void SeedShirtsAndUsers(IServiceProvider services)
        {
            var data = services.GetRequiredService<MyShirtsAppDbContext>();

            if (data.Shirts.Any())
            {
                return;
            }

            var shirtsJsonAsString = File.ReadAllText("./wwwroot/importShirts.json");

            var shirts = JsonConvert.DeserializeObject<IEnumerable<ImportShirtModel>>(shirtsJsonAsString);

            var mapperConfiguration = new MapperConfiguration(cfg =>
                cfg.AddProfile<MappingProfile>());

            var mapper = new Mapper(mapperConfiguration);

            var mappedShirts = mapper.Map<List<Shirt>>(shirts);

            var userManager = services.GetRequiredService<UserManager<User>>();

            Task.Run(async () =>
            {
                if (userManager.Users.Count() > 1)
                {
                    return;
                }

                const string sellerEmail = "seller@msa.bg";
                const string userEmail = "user@msa.bg";
                const string password = "msa123";

                var seller = new User
                {
                    Email = sellerEmail,
                    UserName = sellerEmail,
                    IsSeller = true
                };

                mappedShirts.ForEach(s => s.UserId = seller.Id);

                await userManager.CreateAsync(seller, password);

                data.Shirts.AddRange(mappedShirts);

                data.SaveChanges();

                var user = new User
                {
                    Email = userEmail,
                    UserName = userEmail
                };

                await userManager.CreateAsync(user, password);
            })
                .GetAwaiter()
                .GetResult();
        }
    }
}
