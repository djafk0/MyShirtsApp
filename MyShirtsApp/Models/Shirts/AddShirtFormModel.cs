namespace MyShirtsApp.Models.Shirts
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Shirt;

    public class AddShirtFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Display(Name = "Size")]
        public int SizeId { get; init; }

        [Range((double)MinPrice, (double)MaxPrice)]
        public decimal Price { get; init; }

        [Required]
        [StringLength(FabricMaxLength, MinimumLength = FabricMinLength)]
        public string Fabric { get; set; }

        [Display(Name = "Image Url")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }

        public IEnumerable<ShirtSizeViewModel> Sizes { get; set; }
    }
}
