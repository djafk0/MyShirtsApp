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

        List<int?> GetSizes(AddShirtFormModel shirt);

        int Create(string name, string imageUrl, decimal price, List<int?> sizes);
    }
}
