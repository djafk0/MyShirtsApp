namespace MyShirtsApp.Models.Shirts
{
    using System.ComponentModel.DataAnnotations;

    public class AllShirtsQueryModel
    {
        public string Size { get; init; }

        [Display(Name = "Order by")]
        public ShirtSorting Sorting { get; set; }

        public IEnumerable<string> Sizes { get; set; }

        public IEnumerable<ShirtListingViewModel> Shirts { get; set; }

        public IEnumerable<string> Fabrics { get; init; }
    }
}