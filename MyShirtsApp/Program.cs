using MyShirtsApp.Data;
using MyShirtsApp.Infrastructure;
using MyShirtsApp.Services.Shirts;
using MyShirtsApp.Data.Models;
using MyShirtsApp.Services.Carts;
using MyShirtsApp.Services.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MyShirtsAppDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter()
    .AddAutoMapper(typeof(Program))
    .AddTransient<IShirtService, ShirtService>()
    .AddTransient<ICartService, CartService>()
    .AddTransient<IUserService, UserService>()
    .AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddDefaultIdentity<User>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MyShirtsAppDbContext>();

builder.Services
    .AddMemoryCache()
    .AddControllersWithViews(options
    => options.Filters
        .Add<AutoValidateAntiforgeryTokenAttribute>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error")
       .UseHsts();
}

app
    .PrepareDatabase()
    .UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "shirtDetails",
    pattern: "/Shirts/Details/{id}/{name}",
    defaults: new { controller = "Shirts", action = "Details"});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
