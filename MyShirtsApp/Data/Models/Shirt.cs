namespace MyShirtsApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using static DataConstants.Shirt;

    public class Shirt
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; }

        public ICollection<ShirtSize> ShirtSizes { get; set; } = new List<ShirtSize>();

        public IEnumerable<ShirtCart> Carts { get; set; } = new List<ShirtCart>();
    }
}
