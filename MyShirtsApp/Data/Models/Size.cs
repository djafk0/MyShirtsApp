﻿namespace MyShirtsApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using static DataConstants.Size;

    public class Size
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public ICollection<Shirt> Shirts { get; init; } = new List<Shirt>();
    }
}
