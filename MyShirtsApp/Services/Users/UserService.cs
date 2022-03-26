namespace MyShirtsApp.Services.Users
{
    using MyShirtsApp.Data;

    public class UserService : IUserService
    {
        private readonly MyShirtsAppDbContext data;

        public UserService(MyShirtsAppDbContext data)
            => this.data = data;

        public void BecomeSeller(string userId, string companyName)
        {
            var user = this.data
                .Users
                .FirstOrDefault(u => u.Id == userId);

            user.IsSeller = true;
            user.CompanyName = companyName;

            this.data.SaveChanges();
        }

        public bool IsSeller(string userId)
            => this.data
                .Users
                .Any(u => u.Id == userId && u.IsSeller);
    }
}
