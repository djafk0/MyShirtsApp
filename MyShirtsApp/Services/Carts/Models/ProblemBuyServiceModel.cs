namespace MyShirtsApp.Services.Carts.Models
{
    public class ProblemBuyServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string SizeName { get; set; }

        public int ShirtSizeCount { get; set; }

        public int ShirtCartCount { get; set; }
    }
}
