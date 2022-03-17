namespace MyShirtsApp.Services.Shirts.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.ShirtSize;

    public class SizesServiceModel
    {
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
