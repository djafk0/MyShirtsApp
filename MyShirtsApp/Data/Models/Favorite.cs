namespace MyShirtsApp.Data.Models
{
    public class Favorite
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public ICollection<ShirtFavorite> ShirtFavorites { get; set; } = new List<ShirtFavorite>();
    }
}
