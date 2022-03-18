namespace MyShirtsApp.Models.Carts
{
    public class CartResultViewModel
    {
        public decimal TotalPrice { get; set; }

        public IEnumerable<CartShirtViewModel> Cart { get; set; }
    }
}
