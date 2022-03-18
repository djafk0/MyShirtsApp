namespace MyShirtsApp.Data
{
    using MyShirtsApp.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public class MyShirtsAppDbContext : IdentityDbContext<User>
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

            builder.Entity<ShirtSize>()
                .HasOne(ss => ss.Shirt)
                .WithMany(s => s.ShirtSizes)
                .HasForeignKey(ss => ss.ShirtId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ShirtSize>()
               .HasOne(ss => ss.Size)
               .WithMany(s => s.ShirtSizes)
               .HasForeignKey(ss => ss.SizeId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ShirtCart>()
                .HasKey(x => new { x.ShirtId, x.CartId, x.SizeName });

            builder.Entity<ShirtCart>()
                .HasOne(ss => ss.Shirt)
                .WithMany(s => s.ShirtCarts)
                .HasForeignKey(ss => ss.ShirtId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ShirtCart>()
               .HasOne(ss => ss.Cart)
               .WithMany(s => s.ShirtCarts)
               .HasForeignKey(ss => ss.CartId)
               .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}