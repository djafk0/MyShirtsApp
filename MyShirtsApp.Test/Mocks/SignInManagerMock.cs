namespace MyShirtsApp.Test.Mocks
{
    using MyShirtsApp.Data.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Moq;

    public class SignInManagerMock
    {
        public static SignInManager<User> Instance
        {
            get
            {
                var signInManagerMock = new Mock<SignInManager<User>>(
                    UserManagerMock.Instance,
                    Mock.Of<IHttpContextAccessor>(),
                    Mock.Of<IUserClaimsPrincipalFactory<User>>(),
                    null, null, null, null);

                return signInManagerMock.Object;
            }
        }

    }
}
