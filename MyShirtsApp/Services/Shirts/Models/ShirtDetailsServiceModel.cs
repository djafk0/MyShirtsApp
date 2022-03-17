namespace MyShirtsApp.Services.Shirts.Models
{
    public class ShirtDetailsServiceModel : SizesServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string ImageUrl { get; init; }

        public decimal? Price { get; init; }

        public string UserId { get; init; }
    }
}