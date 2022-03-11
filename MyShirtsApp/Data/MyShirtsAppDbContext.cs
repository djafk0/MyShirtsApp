namespace MyShirtsApp.Data
{
    using MyShirtsApp.Data.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Identity;

    public class MyShirtsAppDbContext : IdentityDbContext
    {
        public MyShirtsAppDbContext(DbContextOptions<MyShirtsAppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Shirt> Shirts { get; init; }

        public DbSet<Size> Sizes { get; init; }

        public DbSet<Cart> Carts { get; init; }

        public DbSet<Seller> Sellers { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Shirt>()
                .HasOne(s => s.Size)
                .WithMany(s => s.Shirts)
                .HasForeignKey(s => s.SizeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Shirt>()
                .HasOne(s => s.Seller)
                .WithMany(s => s.Shirts)
                .HasForeignKey(s => s.SellerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Seller>()
                .HasOne<IdentityUser>()
                .WithOne()
                .HasForeignKey<Seller>(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}