namespace MyShirtsApp.Services.Users
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using Microsoft.AspNetCore.Identity;

    using static WebConstants;

    public class UserService : IUserService
    {
        private readonly MyShirtsAppDbContext data;
        private readonly UserManager<User> userManager;

        public UserService(
            MyShirtsAppDbContext data,
            UserManager<User> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

        public void BecomeSeller(string userId, string companyName)
        {
            var user = this.data
                .Users
                .Find(userId);

            user.IsSeller = true;
            user.CompanyName = companyName;

            Task.Run(async () =>
            {
                await this.userManager.RemoveFromRoleAsync(user, UserRole);
                await this.userManager.AddToRoleAsync(user, SellerRole);
            })
                .GetAwaiter()
                .GetResult();

            this.data.SaveChanges();
        }

        public bool IsSeller(string userId)
            => this.data
                .Users
                .Any(u => u.Id == userId && u.IsSeller);
    }
}
