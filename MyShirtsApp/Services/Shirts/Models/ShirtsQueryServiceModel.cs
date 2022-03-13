namespace MyShirtsApp.Services.Shirts.Models
{
    public class ShirtsQueryServiceModel
    {
        public int TotalShirts { get; init; }

        public IEnumerable<ShirtServiceModel> Shirts { get; init; }
    }
}
