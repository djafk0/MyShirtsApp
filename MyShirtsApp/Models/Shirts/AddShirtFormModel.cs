namespace MyShirtsApp.Models.Shirts
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Shirt;

    public class AddShirtFormModel
    {
        public bool IsValidSize { get; set; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Range((double)MinPrice, (double)MaxPrice)]
        public decimal Price { get; init; }

        [Display(Name = "Image Url")]
        [Required]
        [Url]
        public string ImageUrl { get; init; }

        [Display(Name = "XS")]
        public int? SizeXS { get; init; }

        [Display(Name = "S")]
        public int? SizeS { get; init; }

        [Display(Name = "M")]
        public int? SizeM { get; init; }

        [Display(Name = "L")]
        public int? SizeL { get; init; }

        [Display(Name = "XL")]
        public int? SizeXL { get; init; }

        [Display(Name = "XXL")]
        public int? SizeXXL { get; init; }
    }
}
