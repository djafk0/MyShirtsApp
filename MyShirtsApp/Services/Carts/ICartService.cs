namespace MyShirtsApp.Services.Carts
{
    public interface ICartService
    {
        bool IsAdded(int id, string size, string userId);
    }
}
