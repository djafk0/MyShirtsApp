namespace MyShirtsApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Cart
    {
        public int Id { get; init; }

        [Column(TypeName = "decimal(6,2)")]
        public decimal Total { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<ShirtCart> ShirtCarts { get; set; } = new List<ShirtCart>();
    }
}