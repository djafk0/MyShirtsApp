namespace MyShirtsApp.Services.Shirts
{
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts.Models;

    public interface IShirtService
    {
        ShirtsQueryServiceModel All(
            string size,
            ShirtSorting sorting,
            int currentPage,
            int shirtsPerPage);

        IEnumerable<string> AllShirtSizes();
    }
}
