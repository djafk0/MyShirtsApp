namespace MyShirtsApp.Models.Shirts
{
    using MyShirtsApp.Services.Shirts;
    using System.ComponentModel.DataAnnotations;

    public class AllShirtsQueryModel
    {
        public const int ShirtsPerPage = 3;

        public int CurrentPage { get; set; } = 1;

        public int TotalShirts { get; set; }

        public string Size { get; init; }

        [Display(Name = "Order by")]
        public ShirtSorting Sorting { get; set; }

        public IEnumerable<string> Sizes { get; set; }

        public IEnumerable<ShirtServiceModel> Shirts { get; set; }
    }
}