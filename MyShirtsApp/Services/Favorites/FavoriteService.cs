namespace MyShirtsApp.Services.Favorites
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Services.Shirts;
    using Microsoft.EntityFrameworkCore;

    public class FavoriteService : IFavoriteService
    {
        private readonly MyShirtsAppDbContext data;

        public FavoriteService(MyShirtsAppDbContext data)
            => this.data = data;

        public IEnumerable<ShirtServiceModel> All(string userId)
            => this.data
                .ShirtFavorites
                .Where(x => x.Favorite.UserId == userId)
                .Select(f => new ShirtServiceModel
                {
                    Id = f.ShirtId,
                    Name = f.Shirt.Name,
                    ImageUrl = f.Shirt.ImageUrl,
                    Price = f.Shirt.Price,
                    IsAvailable = !f.Shirt.ShirtSizes.All(ss => ss.Count == 0),
                    IsPublic = f.Shirt.IsPublic,
                    IsFavorite = true
                })
                .ToList();

        public bool IsAdded(int shirtId, string shirtName, string userId)
        {
            bool favoriteExists = true;

            var favorite = this.data
                .Favorites
                .Include(f => f.ShirtFavorites)
                .FirstOrDefault(f => f.UserId == userId);

            if (favorite == null)
            {
                favoriteExists = false;

                favorite = new Favorite { UserId = userId };
            }

            shirtName = shirtName.Replace("-", " ");

            var isValidShirt = this.data
                .Shirts
                .Any(s => s.Id == shirtId && s.Name == shirtName);

            if (!isValidShirt)
            {
                return false;
            }

            var shirtFavorite = favorite.ShirtFavorites
                .FirstOrDefault(sf =>
                    sf.ShirtId == shirtId &&
                    sf.Favorite == favorite);

            if (shirtFavorite != null)
            {
                favorite.ShirtFavorites.Remove(shirtFavorite);
            }
            else
            {
                shirtFavorite = new ShirtFavorite
                {
                    ShirtId = shirtId,
                    Favorite = favorite
                };

                favorite.ShirtFavorites.Add(shirtFavorite);

                if (!favoriteExists)
                {
                    this.data.Favorites.Add(favorite);
                }
            }

            this.data.SaveChanges();

            return true;
        }
    }
}
