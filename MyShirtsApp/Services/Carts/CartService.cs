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

            var shirt = this.GetShirt(id);

            if (shirt == null && !this.CheckSize(sizeName))
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

        public IEnumerable<CartShirtViewModel> MyCart(string userId)
            => this.data
                .ShirtCarts
                .Where(c => c.Cart.UserId == userId)
                .Select(c => new CartShirtViewModel
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

        private ShirtCart GetShirtCart(
            ShirtCartServiceModel shirt,
            Cart cart,
            string sizeName)
            => this.data.
                ShirtCarts
                .FirstOrDefault(s => s.ShirtId == shirt.Id
                    && s.CartId == cart.Id
                    && s.SizeName == sizeName);

        private ShirtCartServiceModel GetShirt(int id)
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

        private bool CheckSize(string sizeName)
            => this.data.
                Sizes
                .Any(s => s.Name == sizeName);
    }
}
