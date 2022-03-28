namespace MyShirtsApp.Services.Favorites
{
    using MyShirtsApp.Services.Shirts;

    public interface IFavoriteService
    {
        IEnumerable<ShirtServiceModel> All(string userId);

        bool IsAdded(int shirtId, string shirtName, string userId);
    }
}
