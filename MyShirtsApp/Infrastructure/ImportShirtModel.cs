namespace MyShirtsApp.Infrastructure
{
    public class ImportShirtModel
    {
        public string Name { get; set; }

        public decimal? Price { get; set; }

        public string ImageUrl { get; set; }

        public bool IsPublic { get; set; }

        public string UserId { get; set; }
    }
}
