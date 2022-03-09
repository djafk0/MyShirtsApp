namespace MyShirtsApp.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using MyShirtsApp.Data.Models;

    public class MyShirtsAppDbContext : IdentityDbContext
    {
        public MyShirtsAppDbContext(DbContextOptions<MyShirtsAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shirt> Shirts { get; init; }

        public DbSet<Size> Sizes { get; init; }

        public DbSet<Cart> Carts { get; init; }
    }
}