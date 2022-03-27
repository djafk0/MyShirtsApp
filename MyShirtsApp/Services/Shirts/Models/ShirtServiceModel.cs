namespace MyShirtsApp.Services.Shirts
{
    public class ShirtServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public decimal? Price { get; init; }

        public bool IsAvailable { get; init; }

        public bool IsPublic { get; init; }
    }
}