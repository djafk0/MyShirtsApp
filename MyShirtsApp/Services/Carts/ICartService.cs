namespace MyShirtsApp.Services.Carts
{
    using MyShirtsApp.Models.Carts;
    using MyShirtsApp.Services.Carts.Models;

    public interface ICartService
    {
        bool IsAdded(int id, string size, string userId);

        ICollection<CartShirtViewModel> MyCart(string userId);

        public bool IsDeletedShirt(
           int shirtId,
           string userId,
           string sizeName,
           bool flag);
    }
}
