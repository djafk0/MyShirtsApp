namespace MyShirtsApp.Models.Shirts
{
    using System.ComponentModel.DataAnnotations;
    using MyShirtsApp.Services.Shirts;

    public class AllShirtsQueryModel
    {
        [Display(Name = "Per Page")]
        public int ShirtsPerPage { get; set; } = 3;

        public int CurrentPage { get; set; } = 1;

        public int TotalShirts { get; set; }

        public int Size { get; set; }

        [Display(Name = "Order")]
        public ShirtSorting Sorting { get; set; }

        public IEnumerable<ShirtServiceModel> Shirts { get; set; }
    }
}