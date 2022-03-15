namespace MyShirtsApp.Data.Models
{
    public class Cart
    {
        public int Id { get; init; }

        public IEnumerable<ShirtCart> Carts { get; set; } = new List<ShirtCart>();
    }
}
