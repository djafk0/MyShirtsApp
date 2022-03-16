namespace MyShirtsApp.Models.Shirts
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Shirt;
    using static Data.DataConstants.ShirtSize;

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

        [Range(MinCount, MaxCount)]
        [Display(Name = "XS")]
        public int? SizeXS { get; init; }

        [Range(MinCount, MaxCount)]
        [Display(Name = "S")]
        public int? SizeS { get; init; }

        [Range(MinCount, MaxCount)]
        [Display(Name = "M")]
        public int? SizeM { get; init; }

        [Range(MinCount, MaxCount)]
        [Display(Name = "L")]
        public int? SizeL { get; init; }

        [Range(MinCount, MaxCount)]
        [Display(Name = "XL")]
        public int? SizeXL { get; init; }

        [Range(MinCount, MaxCount)]
        [Display(Name = "XXL")]
        public int? SizeXXL { get; init; }
    }
}
