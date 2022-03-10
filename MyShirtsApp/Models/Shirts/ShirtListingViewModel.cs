namespace MyShirtsApp.Models.Shirts
{
    public class ShirtListingViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Size { get; init; }

        public decimal Price { get; init; }

        public string Fabric { get; init; }

        public string ImageUrl { get; init; }
    }
}
