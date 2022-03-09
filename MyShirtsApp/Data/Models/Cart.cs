namespace MyShirtsApp.Data.Models
{
    public class Cart
    {
        public int Id { get; init; }

        public ICollection<Shirt> Shirts { get; set; } = new List<Shirt>();
    }
}
