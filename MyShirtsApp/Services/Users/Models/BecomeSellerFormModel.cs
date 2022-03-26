namespace MyShirtsApp.Services.Users.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.User;

    public class BecomeSellerFormModel
    {
        [Required]
        [Display(Name = "Company Name")]
        [StringLength(CompanyNameMaxLength, MinimumLength = CompanyNameMinLength)]
        public string CompanyName { get; set; }
    }
}
