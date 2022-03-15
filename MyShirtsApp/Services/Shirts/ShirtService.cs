﻿namespace MyShirtsApp.Services.Shirts
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts.Models;

    public class ShirtService : IShirtService
    {
        private readonly MyShirtsAppDbContext data;

        public ShirtService(MyShirtsAppDbContext data)
            => this.data = data;

        public ShirtsQueryServiceModel All(
            int sizeId,
            ShirtSorting sorting,
            int currentPage,
            int shirtsPerPage)
        {
            var shirtsQuery = this.data
                   .Shirts
                   .AsQueryable();

            if (sizeId != 0)
            {
                shirtsQuery = shirtsQuery.Where(ss => ss.ShirtSizes.Any(s => s.SizeId == sizeId && s.Count != 0));
            }

            shirtsQuery = sorting switch
            {
                ShirtSorting.Newest => shirtsQuery.OrderByDescending(s => s.Id),
                ShirtSorting.Oldest => shirtsQuery.OrderBy(s => s.Id),
                ShirtSorting.PriceAsc => shirtsQuery.OrderBy(s => s.Price),
                ShirtSorting.PriceDesc or _ => shirtsQuery.OrderByDescending(s => s.Price)
            };

            var totalShirts = shirtsQuery.Count();

            if (currentPage < 1)
            {
                currentPage = 1;
            }

            var shirts = shirtsQuery
                .Skip((currentPage - 1) * shirtsPerPage)
                .Take(shirtsPerPage)
                .Select(s => new ShirtServiceModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    Price = s.Price,
                })
                .ToList();

            return new ShirtsQueryServiceModel
            {
                Shirts = shirts,
                TotalShirts = totalShirts
            };
        }

        public IEnumerable<string> AllShirtSizes()
            => this.data
                .Sizes
                .Select(s => s.Name)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
    }
}
