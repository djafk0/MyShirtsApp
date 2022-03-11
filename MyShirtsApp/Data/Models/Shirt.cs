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

        public int SizeId { get; set; }

        public Size Size { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(FabricMaxLength)]
        public string Fabric { get; set; }

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; }

        public int SellerId { get; init; }

        public Seller Seller { get; init; }

        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
