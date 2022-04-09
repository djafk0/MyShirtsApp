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
        private readonly SignInManager<User> signInManager;

        public UserService(
            MyShirtsAppDbContext data,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            this.data = data;
            this.userManager = userManager;
            this.signInManager = signInManager;
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

                await this.data.SaveChangesAsync();

                await this.signInManager.RefreshSignInAsync(user);
            })
            .GetAwaiter()
            .GetResult();

        }

        public bool IsSeller(string userId)
            => this.data
                .Users
                .Any(u => u.Id == userId && u.IsSeller);
    }
}
