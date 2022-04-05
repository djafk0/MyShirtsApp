namespace MyShirtsApp.Services.Carts
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Models.Carts;
    using MyShirtsApp.Services.Carts.Models;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using AutoMapper;

    public class CartService : ICartService
    {
        private readonly IMapper mapper;
        private readonly MyShirtsAppDbContext data;

        public CartService(IMapper mapper, MyShirtsAppDbContext data)
        {
            this.mapper = mapper;
            this.data = data;
        }

        public bool IsAdded(int id, string sizeName, string userId)
        {
            bool cartExists = true;

            var cart = this.GetCart(userId);

            if (cart == null)
            {
                cartExists = false;

                cart = new Cart { UserId = userId };
            }

            var isValidShirtId = this.data
                .Shirts
                .Any(s => s.Id == id);

            var isValidSizeName = this.data
                .Sizes
                .Any(s => s.Name == sizeName);

            if (!isValidShirtId || !isValidSizeName)
            {
                return false;
            }

            var shirtCart = cart.ShirtCarts
                .FirstOrDefault(sc =>
                    sc.ShirtId == id &&
                    sc.Cart == cart &&
                    sc.SizeName == sizeName);

            if (shirtCart != null)
            {
                shirtCart.Count++;
            }
            else
            {
                shirtCart = new ShirtCart
                {
                    ShirtId = id,
                    Cart = cart,
                    Count = 1,
                    SizeName = sizeName
                };

                cart.ShirtCarts.Add(shirtCart);

                if (!cartExists)
                {
                    this.data.Carts.Add(cart);
                }
            }

            this.data.SaveChanges();

            return true;
        }

        public IEnumerable<CartShirtServiceModel> MyCart(string userId)
            => this.data
                .ShirtCarts
                .Where(c => c.Cart.UserId == userId)
                .ProjectTo<CartShirtServiceModel>(this.mapper.ConfigurationProvider)
                .ToList();

        public bool IsDeletedShirt(
            int shirtId,
            string userId,
            string sizeName,
            bool flag)
        {
            var shirtCart = this.data
                .ShirtCarts
                .Where(sc =>
                    sc.ShirtId == shirtId &&
                    sc.Cart.UserId == userId &&
                    sc.SizeName == sizeName)
                .Include(sc => sc.Cart)
                .Include(sc => sc.Shirt)
                .ThenInclude(s => s.ShirtSizes)
                .ThenInclude(ss => ss.Size)
                .FirstOrDefault();

            if (shirtCart == null)
            {
                return false;
            }

            if (flag || shirtCart.Count == 1)
            {
                this.data.ShirtCarts.Remove(shirtCart);
            }
            else
            {
                var sizeId = shirtCart.Shirt
                    .ShirtSizes
                    .FirstOrDefault(x => 
                        x.Size.Name == sizeName)
                    .SizeId;

                var shirtSize = shirtCart.Shirt
                    .ShirtSizes
                    .FirstOrDefault(s =>
                        s.ShirtId == shirtId &&
                        s.SizeId == sizeId);

                if (shirtCart.Count > shirtSize.Count)
                {
                    shirtCart.Count = (int)shirtSize.Count;
                }
            }

            this.data.SaveChanges();

            return true;
        }

        public void ClearCart(string userId)
        {
            var cart = this.GetCart(userId);

            if (cart == null)
            {
                return;
            }

            cart.ShirtCarts.Clear();

            this.data.SaveChanges();
        }

        public IEnumerable<ProblemBuyServiceModel> BuyAll(string userId)
        {
            var problems = new List<ProblemBuyServiceModel>();

            var cart = this.data
                .Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.ShirtCarts)
                .ThenInclude(sc => sc.Shirt)
                .ThenInclude(s => s.ShirtSizes)
                .ThenInclude(ss => ss.Size)
                .FirstOrDefault();

            if (cart == null)
            {
                return problems;
            }

            foreach (var shirtCart in cart.ShirtCarts)
            {
                var shirt = shirtCart.Shirt;

                var sizeName = shirtCart.SizeName;

                var shirtSize = shirt.ShirtSizes
                    .FirstOrDefault(sc =>
                        sc.Shirt == shirt &&
                        sc.Size.Name == sizeName);

                if (shirtSize.Count < shirtCart.Count)
                {
                    var problem = new ProblemBuyServiceModel
                    {
                        Id = shirt.Id,
                        Name = shirt.Name,
                        ImageUrl = shirt.ImageUrl,
                        SizeName = shirtCart.SizeName,
                        ShirtSizeCount = (int)shirtSize.Count,
                        ShirtCartCount = shirtCart.Count
                    };

                    problems.Add(problem);
                }
                else
                {
                    shirtSize.Count -= shirtCart.Count;
                }
            }

            if (!problems.Any())
            {
                this.data.SaveChanges();
            }

            return problems;
        }

        private Cart GetCart(string userId)
            => this.data
                .Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.ShirtCarts)
                .FirstOrDefault();
    }
}
