namespace MyShirtsApp.Data.Models
{
    public class ShirtFavorite
    {
        public int ShirtId { get; set; }

        public Shirt Shirt { get; set; }

        public int FavoriteId { get; set; }

        public Favorite Favorite { get; set; }
    }
}
