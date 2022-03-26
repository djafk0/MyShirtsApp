namespace MyShirtsApp.Services.Users
{
    public interface IUserService
    {
        void BecomeSeller(string userId, string companyName);

        public bool IsSeller(string userId);
    }
}
