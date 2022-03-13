namespace MyShirtsApp.Services.Shirts
{
    public class ShirtServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Fabric { get; init; }

        public string ImageUrl { get; init; }

        public decimal Price { get; init; }

        public string Size { get; init; }
    }
}
