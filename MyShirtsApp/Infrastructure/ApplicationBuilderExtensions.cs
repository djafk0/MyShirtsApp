﻿namespace MyShirtsApp.Infrastructure
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<MyShirtsAppDbContext>();

            data.Database.Migrate();

            SeedSizes(data);

            return app;
        }

        private static void SeedSizes(MyShirtsAppDbContext data)
        {
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
    }
}
