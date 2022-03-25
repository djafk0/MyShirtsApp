namespace MyShirtsApp.Services.Users
{
    using MyShirtsApp.Data;

    public class UserService : IUserService
    {
        private readonly MyShirtsAppDbContext data;

        public UserService(MyShirtsAppDbContext data)
            => this.data = data;

        public bool BecomeSeller(string userId, string companyName)
        {
            var user = this.data
                .Users
                .FirstOrDefault(u => u.Id == userId);

            if (user.IsSeller)
            {
                return true;
            }

            user.IsSeller = true;
            user.CompanyName = companyName;

            this.data.SaveChanges();

            return false;
        }

        public bool IsSeller(string userId)
            => this.data
                .Users
                .Any(u => u.Id == userId && u.IsSeller);
    }
}
