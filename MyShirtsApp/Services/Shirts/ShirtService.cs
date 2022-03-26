﻿namespace MyShirtsApp.Services.Shirts
{
    using System.Collections.Generic;
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts.Models;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using AutoMapper;

    public class ShirtService : IShirtService
    {
        private readonly IMapper mapper;
        private readonly MyShirtsAppDbContext data;

        public ShirtService(IMapper mapper, MyShirtsAppDbContext data)
        {
            this.mapper = mapper;
            this.data = data;
        }

        public ShirtsQueryServiceModel All(
            int size,
            ShirtSorting sorting,
            int currentPage,
            int shirtsPerPage)
        {
            var shirtsQuery = this.data
                   .Shirts
                   .AsQueryable();

            if (size >= 1 && size <= 6)
            {
                shirtsQuery = shirtsQuery
                    .Where(ss => ss.ShirtSizes
                        .Any(s => s.SizeId == size && s.Count > 0));
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

            var shirts = this.GetShirts(shirtsQuery
                .Skip((currentPage - 1) * shirtsPerPage)
                .Take(shirtsPerPage));

            return new ShirtsQueryServiceModel
            {
                Shirts = shirts,
                TotalShirts = totalShirts
            };
        }

        public ShirtDetailsServiceModel Details(int id)
            => this.data
                  .Shirts
                  .Where(s => s.Id == id)
                  .ProjectTo<ShirtDetailsServiceModel>(this.mapper.ConfigurationProvider)
                  .FirstOrDefault();

        public IEnumerable<ShirtServiceModel> ShirtsByUser(string userId)
            => this.GetShirts(this.data
                .Shirts
                .Where(s => s.UserId == userId));

        public List<int?> SizesFromModel(ShirtFormModel shirt)
            => new()
            {
                shirt.SizeXS ?? 0,
                shirt.SizeS ?? 0,
                shirt.SizeM ?? 0,
                shirt.SizeL ?? 0,
                shirt.SizeXL ?? 0,
                shirt.SizeXXL ?? 0
            };

        public int Create(
            string name,
            string imageUrl,
            decimal? price,
            string userId,
            List<int?> sizes)
        {
            var shirtData = new Shirt
            {
                Name = name,
                ImageUrl = imageUrl,
                Price = price,
                UserId = userId,
            };

            for (int i = 1; i <= 6; i++)
            {
                shirtData.ShirtSizes.Add(new ShirtSize
                {
                    SizeId = i,
                    Count = sizes[i - 1]
                });
            }

            this.data.Shirts.Add(shirtData);
            this.data.SaveChanges();

            return shirtData.Id;
        }

        public bool Edit(
            int id,
            string name,
            string imageUrl,
            decimal? price,
            string userId,
            bool isAdmin,
            List<int?> sizes)
        {
            var shirtData = this.data
                .Shirts
                .Include(i => i.ShirtSizes)
                .FirstOrDefault(s => s.Id == id);

            if (shirtData == null)
            {
                return false;
            }

            if (shirtData.UserId != userId && !isAdmin)
            {
                return false;
            }

            shirtData.Name = name;
            shirtData.ImageUrl = imageUrl;
            shirtData.Price = price;

            var i = 0;

            foreach (var shirtSize in shirtData.ShirtSizes)
            {
                shirtSize.Count = sizes[i++];
            }

            this.data.SaveChanges();

            return true;
        }

        public bool Delete(int id, string userId, bool isAdmin)
        {
            var shirt = this.data
                .Shirts
                .Where(s => s.Id == id)
                .Include(s => s.ShirtSizes)
                .Include(s => s.ShirtCarts)
                .FirstOrDefault();

            if (shirt == null)
            {
                return false;
            }

            if (shirt.UserId != userId && !isAdmin)
            {
                return false;
            }

            shirt.ShirtCarts.Clear();
            shirt.ShirtSizes.Clear();

            this.data.Shirts.Remove(shirt);
            this.data.SaveChanges();

            return true;
        }

        private IEnumerable<ShirtServiceModel> GetShirts(IQueryable<Shirt> shirtQuery)
            => shirtQuery
                .ProjectTo<ShirtServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();
    }
}