namespace MyShirtsApp.Models.Shirts
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;
    public class AddShirtFormModel
    {
        [Required]
        [MaxLength(ShirtNameMaxLength)]
        public string Name { get; init; }

        [Display(Name = "Size")]
        public int SizeId { get; init; }

        [Range((double)ShirtMinPrice, (double)ShirtMaxPrice)]
        public decimal Price { get; init; }

        [Display(Name = "Image Url")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }

        public IEnumerable<ShirtSizeViewModel> Sizes { get; set; }
    }
}
