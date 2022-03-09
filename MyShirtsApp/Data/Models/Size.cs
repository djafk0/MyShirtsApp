namespace MyShirtsApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class Size
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(SizeNameMaxLength)]
        public string Name { get; set; }

        public ICollection<Shirt> Shirts { get; init; } = new List<Shirt>();
    }
}
