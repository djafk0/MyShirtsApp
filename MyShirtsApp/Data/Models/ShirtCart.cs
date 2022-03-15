namespace MyShirtsApp.Data.Models
{
    public class ShirtCart
    {
        public int ShirtId { get; set; }

        public Shirt Shirt { get; set; }

        public int CartId { get; set; }

        public Cart Cart { get; set; }
    }
}
