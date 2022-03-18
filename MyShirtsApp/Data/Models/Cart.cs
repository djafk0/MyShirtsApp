namespace MyShirtsApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Cart
    {
        public int Id { get; init; }

        [Required]
        public string UserId { get; set; }

        public ICollection<ShirtCart> ShirtCarts { get; set; } = new List<ShirtCart>();
    }
}