namespace MyShirtsApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Seller;

    public class Seller
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(PhoneNumberMaxLength)]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserId { get; init; }

        public ICollection<Shirt> Shirts { get; init; } = new List<Shirt>();
    }
}
