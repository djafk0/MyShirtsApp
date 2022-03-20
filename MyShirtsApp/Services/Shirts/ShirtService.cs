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
        {
            var shirtSizes = this.data
                .ShirtSizes
                .Where(ss => ss.ShirtId == id)
                .Select(ss => new
                {
                    Name = ss.Size.Name,
                    Count = ss.Count
                })
                .ToList();

            var sizes = new List<SizesServiceModel>();

            if (shirtSizes != null)
            {
                for (int i = 0; i < 6; i++)
                {
                    var size = new SizesServiceModel
                    {
                        SizeName = shirtSizes[i].Name,
                        Count = shirtSizes[i].Count == 0 
                            ? null 
                            : shirtSizes[i].Count
                    };

                    sizes.Add(size);
                }
            }

            var model = this.data
                .Shirts
                .Where(s => s.Id == id)
                 .Select(s => new ShirtDetailsServiceModel
                 {
                     Id = s.Id,
                     Name = s.Name,
                     ImageUrl = s.ImageUrl,
                     Price = s.Price,
                     UserId = s.UserId,
                     Sizes = sizes
                 })
                .FirstOrDefault();

            return model;
        }

        public IEnumerable<ShirtServiceModel> ShirtsByUser(string userId)
            => this.GetShirts(this.data
                .Shirts
                .Where(s => s.UserId == userId));

        public List<int?> SizesFromModel(ShirtFormModel shirt)
            => new List<int?>
            {
                shirt.Sizes[0].Count ?? 0,
                shirt.Sizes[1].Count ?? 0,
                shirt.Sizes[2].Count ?? 0,
                shirt.Sizes[3].Count ?? 0,
                shirt.Sizes[4].Count ?? 0,
                shirt.Sizes[5].Count ?? 0
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

            var shirtSizes = this.GetShirtSizes(id);

            foreach (var shirtSize in shirtSizes)
            {
                shirt.ShirtSizes.Remove(shirtSize);
            }

            var shirtCarts = this.GetShirtCarts(id);

            foreach (var shirtCart in shirtCarts)
            {
                shirt.ShirtCarts.Remove(shirtCart);
            }

            this.data.Shirts.Remove(shirt);

            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<SizesServiceModel> GetSizesAsModel()
            => this.data
                .Sizes
                .Select(s => new SizesServiceModel
                {
                    SizeId = s.Id,
                    SizeName = s.Name
                })
                .ToList();

        private IEnumerable<ShirtServiceModel> GetShirts(IQueryable<Shirt> shirtQuery)
            => shirtQuery
                .Select(s => new ShirtServiceModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    Price = s.Price,
                    Available = !s.ShirtSizes
                        .All(ss => ss.Count == 0)
                })
                .ToList();

        private Shirt GetShirt(int id)
            => this.data
                .Shirts
                .Include(s => s.ShirtSizes)
                .Include(s => s.ShirtCarts)
                .FirstOrDefault(s => s.Id == id);

        private IEnumerable<ShirtSize> GetShirtSizes(int shirtId)
            => this
                .data
                .ShirtSizes
                .Where(s =>
                    s.Shirt.Id == shirtId)
                .ToList();

        private IEnumerable<ShirtCart> GetShirtCarts(int id)
            => this.data
                .ShirtCarts
                .Where(x =>
                    x.ShirtId == id)
                .ToList();
    }
}