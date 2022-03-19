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

            var shirt = this.GetShirtAsModel(id);

            if (shirt == null && this.GetSize(sizeName) != null)
            {
                return false;
            }

            var shirtCart = this.GetShirtCart(shirt, cart, sizeName);

            if (shirtCart != null)
            {
                shirtCart.Count++;
            }
            else
            {
                shirtCart = new ShirtCart
                {
                    ShirtId = shirt.Id,
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
            var cart = this.GetCart(userId);

            var shirt = this.GetShirtAsModel(shirtId);

            var shirtCart = GetShirtCart(shirt, cart, sizeName);

            if (cart == null || shirt == null || shirtCart == null)
            {
                return false;
            }

            if (flag || shirtCart.Count == 1)
            {
                cart.ShirtCarts.Remove(shirtCart);
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

            var cart = this.GetCart(userId);

            foreach (var shirtCart in cart.ShirtCarts)
            {
                var shirt = this.GetShirt(shirtCart.ShirtId);

                var shirtSize = this.GetShirtSize(shirt, shirtCart.SizeName);

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

        private ShirtCart GetShirtCart(
            ShirtCartServiceModel shirt,
            Cart cart,
            string sizeName)
            => this.data
                .ShirtCarts
                .FirstOrDefault(s =>
                    s.ShirtId == shirt.Id
                    && s.CartId == cart.Id
                    && s.SizeName == sizeName);

        private ShirtCartServiceModel GetShirtAsModel(int id)
            => this.data
                .Shirts
                .Where(s => s.Id == id)
                .Select(s => new ShirtCartServiceModel
                {
                    Id = s.Id,
                    Price = (decimal)s.Price
                })
                .FirstOrDefault();

        private Cart GetCart(string userId)
            => this.data
                .Carts
                .Include(c => c.ShirtCarts)
                .FirstOrDefault(x => x.UserId == userId);

        private Size GetSize(string sizeName)
            => this.data
                .Sizes
                .FirstOrDefault(s => s.Name == sizeName);

        private Shirt GetShirt(int id)
            => this.data
                .Shirts
                .FirstOrDefault(s => s.Id == id);

        private ShirtSize GetShirtSize(Shirt shirt, string sizeName)
            => this.data
                .ShirtSizes
                .FirstOrDefault(ss =>
                ss.Shirt == shirt
                && ss.Size.Name == sizeName);
    }
}
