namespace MyShirtsApp.Services.Users.Models
{
    using System.ComponentModel.DataAnnotations;

    public class BecomeSellerFormModel
    {
        [Required]
        public string CompanyName { get; set; }
    }
}
