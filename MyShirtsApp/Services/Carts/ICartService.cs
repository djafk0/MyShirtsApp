namespace MyShirtsApp.Services.Carts
{
    using MyShirtsApp.Models.Carts;

    public interface ICartService
    {
        bool IsAdded(int id, string size, string userId);

        IEnumerable<CartShirtViewModel> MyCart(string userId);
    }
}
