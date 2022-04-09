namespace MyShirtsApp.Test.Mocks
{
    using System.Collections.Generic;
    using System.Security.Claims;

    public class ClaimsPrincipalMock
    {
        public static ClaimsPrincipal Instance(string userId = "TestUser")
        {
            var fakeClaims = new List<Claim>()
                {
                   new Claim(ClaimTypes.NameIdentifier, userId),
                };

            var fakeIdentity = new ClaimsIdentity(fakeClaims, "TestAuthType");

            var fakeClaimsPrincipal = new ClaimsPrincipal(fakeIdentity);

            return fakeClaimsPrincipal;
        }
    }
}
