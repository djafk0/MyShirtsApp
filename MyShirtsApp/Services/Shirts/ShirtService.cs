namespace MyShirtsApp.Services.Shirts
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts.Models;
    using System.Collections.Generic;

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

            var shirts = shirtsQuery
                .Skip((currentPage - 1) * shirtsPerPage)
                .Take(shirtsPerPage)
                .Select(s => new ShirtServiceModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    ImageUrl = s.ImageUrl,
                    Price = s.Price,
                    Size = size
                })
                .ToList();

            return new ShirtsQueryServiceModel
            {
                Shirts = shirts,
                TotalShirts = totalShirts
            };
        }

        public List<int?> GetSizes(AddShirtFormModel shirt)
            => new List<int?>
            {
                shirt.SizeXS ?? 0,
                shirt.SizeS ?? 0,
                shirt.SizeM ?? 0,
                shirt.SizeL ?? 0,
                shirt.SizeXL ?? 0,
                shirt.SizeXXL ?? 0
            };

        public int Create(string name, string imageUrl, decimal price, List<int?> sizes)
        {
            var shirtData = new Shirt
            {
                Name = name,
                ImageUrl = imageUrl,
                Price = price,
            };

            for (int i = 1; i <= 6; i++)
            {
                shirtData.ShirtSizes.Add(new ShirtSize { SizeId = i, Count = sizes[i - 1] });
            }

            this.data.Shirts.Add(shirtData);
            this.data.SaveChanges();

            return shirtData.Id;
        }
    }
}