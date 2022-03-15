namespace MyShirtsApp.Data
{
    using MyShirtsApp.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class MyShirtsAppDbContext : IdentityDbContext
    {
        public MyShirtsAppDbContext(DbContextOptions<MyShirtsAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shirt> Shirts { get; init; }

        public DbSet<Size> Sizes { get; init; }

        public DbSet<Cart> Carts { get; init; }

        public DbSet<ShirtSize> ShirtSizes { get; init; }

        public DbSet<ShirtCart> ShirtCarts { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<ShirtSize>()
                .HasKey(x => new { x.ShirtId, x.SizeId });

            builder
                .Entity<ShirtCart>()
                .HasKey(x => new { x.ShirtId, x.CartId });

            base.OnModelCreating(builder);
        }
    }
}