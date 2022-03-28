namespace MyShirtsApp.Services.Favorites
{
    using MyShirtsApp.Data;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Services.Shirts;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper.QueryableExtensions;
    using AutoMapper;

    public class FavoriteService : IFavoriteService
    {
        private readonly MyShirtsAppDbContext data;
        private readonly IMapper mapper;

        public FavoriteService(MyShirtsAppDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper;
        }

        public IEnumerable<ShirtServiceModel> All(string userId)
            => this.data
                .Shirts
                .Where(s => s.ShirtFavorites.Any(sf => sf.Favorite.UserId == userId))
                .ProjectTo<ShirtServiceModel>(this.mapper.ConfigurationProvider)
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
