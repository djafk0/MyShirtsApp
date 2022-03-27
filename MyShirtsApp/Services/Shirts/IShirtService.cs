namespace MyShirtsApp.Services.Shirts
{
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts.Models;

    public interface IShirtService
    {
        ShirtsQueryServiceModel All(
            int size = 0,
            ShirtSorting sorting = ShirtSorting.Newest,
            int currentPage = 1,
            int shirtsPerPage = int.MaxValue,
            bool publicOnly = true);

        ShirtDetailsServiceModel Details(int id);

        IEnumerable<ShirtServiceModel> ShirtsByUser(string userId);

        List<int?> SizesFromModel(ShirtFormModel shirt);

        void ChangeVisibility(int id);

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

        bool Delete(int id, string userId, bool isAdmin);
    }
}
