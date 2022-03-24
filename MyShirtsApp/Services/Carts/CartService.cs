namespace MyShirtsApp.Services.Carts
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Models.Carts;
    using MyShirtsApp.Services.Carts.Models;
    using Microsoft.EntityFrameworkCore;

    public class CartService : ICartService
    {
        private readonly MyShirtsAppDbContext data;

        public CartService(MyShirtsAppDbContext data)
            => this.data = data;

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

            if (!isValidShirtId || this.Size(sizeName) == null)
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

        public ICollection<CartShirtServiceModel> MyCart(string userId)
            => this.data
                .ShirtCarts
                .Where(c => c.Cart.UserId == userId)
                .Select(c => new CartShirtServiceModel
                {
                    ShirtId = c.ShirtId,
                    Name = c.Shirt.Name,
                    ImageUrl = c.Shirt.ImageUrl,
                    Price = (decimal)c.Shirt.Price,
                    Count = c.Count,
                    SizeName = c.SizeName,
                    UserId = userId
                })
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
                shirtCart.Count--;
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

            var shirtCarts = cart.ShirtCarts.ToList();

            for (int i = 0; i < shirtCarts.Count; i++)
            {
                cart.ShirtCarts.Remove(shirtCarts[i]);
            }

            this.data.SaveChanges();
        }

        public ICollection<ProblemBuyServiceModel> BuyAll(string userId)
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

        private Size Size(string sizeName)
            => this.data
                .Sizes
                .FirstOrDefault(s => s.Name == sizeName);
    }
}
