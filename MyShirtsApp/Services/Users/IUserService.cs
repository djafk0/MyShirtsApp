namespace MyShirtsApp.Services.Users
{
    public interface IUserService
    {
        bool BecomeSeller(string userId, string companyName);

        public bool IsSeller(string userId);
    }
}
