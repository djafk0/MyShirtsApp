namespace MyShirtsApp.Infrastructure
{
    using System.Security.Claims;

    public static class ClaimsPrincipalsExtensions
    {
        public static string GetId(this ClaimsPrincipal user)
            => user.FindFirst(ClaimTypes.NameIdentifier).Value;
    }
}
