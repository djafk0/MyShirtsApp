namespace MyShirtsApp.Data.Models
{
    public class ShirtSize
    {
        public int ShirtId { get; set; }

        public Shirt Shirt { get; set; }

        public int SizeId { get; set; }

        public Size Size { get; set; }

        public int? Count { get; set; }
    }
}
