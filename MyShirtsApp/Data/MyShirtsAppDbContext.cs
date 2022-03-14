﻿namespace MyShirtsApp.Data
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}