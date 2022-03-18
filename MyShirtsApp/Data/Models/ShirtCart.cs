namespace MyShirtsApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Size;

    public class ShirtCart
    {
        public int ShirtId { get; set; }

        public Shirt Shirt { get; set; }

        public int CartId { get; set; }

        public Cart Cart { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string SizeName { get; set; }

        public int Count { get; set; }
    }
}
