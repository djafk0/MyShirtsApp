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

        public DbSet<Favorite> Favorites { get; init; }

        public DbSet<ShirtFavorite> ShirtFavorites { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<ShirtSize>()
                .HasKey(ss => new { ss.ShirtId, ss.SizeId });

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
                .HasKey(sc => new { sc.ShirtId, sc.CartId, sc.SizeName });

            builder.Entity<ShirtCart>()
                .HasOne(sc => sc.Shirt)
                .WithMany(s => s.ShirtCarts)
                .HasForeignKey(sc => sc.ShirtId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ShirtCart>()
               .HasOne(sc => sc.Cart)
               .WithMany(s => s.ShirtCarts)
               .HasForeignKey(sc => sc.CartId)
               .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<ShirtFavorite>()
                .HasKey(sf => new { sf.ShirtId, sf.FavoriteId });

            builder.Entity<ShirtFavorite>()
                .HasOne(sf => sf.Shirt)
                .WithMany(s => s.ShirtFavorites)
                .HasForeignKey(sf => sf.ShirtId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ShirtFavorite>()
               .HasOne(sf => sf.Favorite)
               .WithMany(s => s.ShirtFavorites)
               .HasForeignKey(sf => sf.FavoriteId)
               .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}