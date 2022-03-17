namespace MyShirtsApp.Services.Shirts
{
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts.Models;

    public interface IShirtService
    {
        ShirtsQueryServiceModel All(
            int size,
            ShirtSorting sorting,
            int currentPage,
            int shirtsPerPage);

        ShirtDetailsServiceModel Details(int id);

        IEnumerable<ShirtServiceModel> ShirtsByUser(string userId);

        List<int?> SizesFromModel(ShirtFormModel shirt);

        int Create(
            string name,
            string imageUrl,
            decimal? price, 
            string userId,
            List<int?> sizes);

        bool Edit(
            int id,
            string name,
            string imageUrl, 
            decimal? price, 
            string userId,
            bool isAdmin,
            List<int?> sizes);
    }
}
