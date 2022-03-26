namespace MyShirtsApp.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;

    using static DataConstants.User;

    public class User : IdentityUser
    {
        [MaxLength(CompanyNameMaxLength)]
        public string CompanyName { get; set; }

        public bool IsSeller { get; set; }
    }
}
