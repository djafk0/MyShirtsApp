namespace MyShirtsApp.Test.Mocks
{
    using Microsoft.AspNetCore.Identity;
    using Moq;
    using MyShirtsApp.Data.Models;

    public class UserManagerMock
    {
        public static UserManager<User> Instance
        {
            get
            {
                var store = new Mock<IUserStore<User>>();
                var mgr = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
                mgr.Object.UserValidators.Add(new UserValidator<User>());
                mgr.Object.PasswordValidators.Add(new PasswordValidator<User>());

                mgr.Setup(x => x.DeleteAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);
                mgr.Setup(x => x.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

                return mgr.Object;
            }
        }
    }
}
