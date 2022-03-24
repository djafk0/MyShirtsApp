namespace MyShirtsApp.Models.Shirts
{
    using System.ComponentModel.DataAnnotations;
    using MyShirtsApp.Services.Shirts.Models;

    using static Data.DataConstants.Shirt;

    public class ShirtFormModel : SizesServiceModel
    {
        public bool IsValidSize { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Required]
        [Range((double)MinPrice, (double)MaxPrice)]
        public decimal? Price { get; init; }

        [Display(Name = "Image Url")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }
    }
}
