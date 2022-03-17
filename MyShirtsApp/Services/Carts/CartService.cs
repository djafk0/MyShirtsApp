namespace MyShirtsApp.Services.Carts
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Services.Carts.Models;

    public class CartService : ICartService
    {
        private readonly MyShirtsAppDbContext data;

        public CartService(MyShirtsAppDbContext data)
            => this.data = data;

        public bool IsAdded(int id, string size, string userId)
        {
            var cart = this.data
                .Carts
                .FirstOrDefault(x => x.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId };
            }

            var shirt = this.data
                .Shirts
                .Where(s => s.Id == id)
                .Select(s => new AddShirtServiceModel
                {
                    Id = s.Id,
                    Price = (decimal)s.Price
                })
                .FirstOrDefault();

            var shirtCart = new ShirtCart
            {
                ShirtId = shirt.Id,
                Size = size
            };

            if (shirt == null && !this.data.Sizes.Any(s => s.Name == size))
            {
                return false;
            }

            if (IsShirtCartExist(shirt, cart))
            {
                shirtCart.Count++;
            }
            else
            {
                cart.Total += shirt.Price;

                cart.ShirtCarts.Add(shirtCart);
            
                this.data.Carts.Add(cart);
            }

            this.data.SaveChanges();

            return true;
        }

        private bool IsShirtCartExist(
            AddShirtServiceModel shirt,
            Cart cart)
            => this.data.
                ShirtCarts
                .Any(s => s.ShirtId == shirt.Id
                    && s.CartId == cart.Id);
    }
}
