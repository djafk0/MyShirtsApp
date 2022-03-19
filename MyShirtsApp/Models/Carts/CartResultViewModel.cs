namespace MyShirtsApp.Models.Carts
{
    public class CartResultViewModel
    {
        public decimal TotalPrice { get; set; }

        public IEnumerable<CartShirtServiceModel> Cart { get; set; }
    }
}
