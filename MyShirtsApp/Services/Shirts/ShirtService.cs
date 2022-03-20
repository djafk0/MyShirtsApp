namespace MyShirtsApp.Services.Shirts
{
    using System.Collections.Generic;
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts.Models;
    using Microsoft.EntityFrameworkCore;

    public class ShirtService : IShirtService
    {
        private readonly MyShirtsAppDbContext data;

        public ShirtService(MyShirtsAppDbContext data)
            => this.data = data;

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
                .Select(s => new ShirtDetailsServiceModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    Price = s.Price,
                    UserId = s.UserId,
                    SizeXS = s.ShirtSizes.First(x => x.SizeId == 1).Count == 0
                        ? null : s.ShirtSizes.First(x => x.SizeId == 1).Count,
                    SizeS = s.ShirtSizes.First(x => x.SizeId == 2).Count == 0
                        ? null : s.ShirtSizes.First(x => x.SizeId == 2).Count,
                    SizeM = s.ShirtSizes.First(x => x.SizeId == 3).Count == 0
                        ? null : s.ShirtSizes.First(x => x.SizeId == 3).Count,
                    SizeL = s.ShirtSizes.First(x => x.SizeId == 4).Count == 0
                        ? null : s.ShirtSizes.First(x => x.SizeId == 4).Count,
                    SizeXL = s.ShirtSizes.First(x => x.SizeId == 5).Count == 0
                        ? null : s.ShirtSizes.First(x => x.SizeId == 5).Count,
                    SizeXXL = s.ShirtSizes.First(x => x.SizeId == 6).Count == 0
                        ? null : s.ShirtSizes.First(x => x.SizeId == 6).Count
                })
                .FirstOrDefault();

        public IEnumerable<ShirtServiceModel> ShirtsByUser(string userId)
            => this.GetShirts(this.data
                .Shirts
                .Where(s => s.UserId == userId));

        public List<int?> SizesFromModel(ShirtFormModel shirt)
            => new List<int?>
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
                UserId = userId
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

            if (shirtData.UserId != userId && !isAdmin)
            {
                return false;
            }

            shirtData.Name = name;
            shirtData.ImageUrl = imageUrl;
            shirtData.Price = price;

            for (int i = 1; i <= 6; i++)
            {
                shirtData.ShirtSizes
                    .FirstOrDefault(ss =>
                        ss.SizeId == i)
                    .Count = sizes[i - 1];
            }

            this.data.SaveChanges();

            return true;
        }

        public bool Delete(int id, string userId, bool isAdmin)
        {
            var shirt = this.GetShirt(id);

            if (shirt == null)
            {
                return false;
            }

            if (shirt.UserId != userId && !isAdmin)
            {
                return false;
            }

            var sizes = this.GetSizes();

            foreach (var size in sizes)
            {
                var shirtSize = this.GetShirtSize(id, size.Id);

                shirt.ShirtSizes.Remove(shirtSize);
            }

            var shirtCarts = this.GetShirtCarts(id);

            foreach (var shirtCart in shirtCarts)
            {
                shirt.ShirtCarts.Remove(shirtCart);
            }

            this.data.SaveChanges();

            this.data.Shirts.Remove(shirt);

            this.data.SaveChanges();

            return true;
        }

        private IEnumerable<ShirtServiceModel> GetShirts(IQueryable<Shirt> shirtQuery)
            => shirtQuery
                .Select(s => new ShirtServiceModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    Price = s.Price,
                    Available = !s.ShirtSizes.All(ss => ss.Count == 0)
                })
                .ToList();

        private Shirt GetShirt(int id)
            => this.data
                .Shirts
                .Include(s => s.ShirtSizes)
                .Include(s => s.ShirtCarts)
                .FirstOrDefault(s => s.Id == id);

        private IEnumerable<Size> GetSizes()
            => this.data
                    .Sizes
                    .ToList();

        private ShirtSize GetShirtSize(int shirtId, int sizeId)
            => this
                .data
                .ShirtSizes
                .FirstOrDefault(s =>
                    s.Shirt.Id == shirtId &&
                    s.Size.Id == sizeId);

        private IEnumerable<ShirtCart> GetShirtCarts(int id)
            => this.data
                .ShirtCarts
                .Where(x =>
                    x.ShirtId == id)
                .ToList();
    }
}