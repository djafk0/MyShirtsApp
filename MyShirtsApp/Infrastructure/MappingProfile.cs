namespace MyShirtsApp.Infrastructure
{
    using MyShirtsApp.Models.Shirts;
    using MyShirtsApp.Services.Shirts.Models;
    using MyShirtsApp.Data.Models;
    using MyShirtsApp.Services.Shirts;
    using MyShirtsApp.Models.Carts;
    using AutoMapper;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<ShirtDetailsServiceModel, ShirtFormModel>();

            this.CreateMap<Shirt, ShirtServiceModel>()
                .ForMember(s => s.IsAvailable, cfg => cfg.MapFrom(m => !m.ShirtSizes.All(ss => ss.Count == 0)));

            this.CreateMap<ShirtCart, CartShirtServiceModel>()
                .ForMember(sc => sc.Name, cfg => cfg.MapFrom(m => m.Shirt.Name))
                .ForMember(sc => sc.ImageUrl, cfg => cfg.MapFrom(m => m.Shirt.ImageUrl))
                .ForMember(sc => sc.Price, cfg => cfg.MapFrom(m => (decimal)m.Shirt.Price));

            this.CreateMap<Shirt, ShirtDetailsServiceModel>()
                .ForMember(s => s.IsAvailable, cfg => cfg.MapFrom(m => m.ShirtSizes.Any(ss => ss.Count > 0)))
                .ForMember(s => s.SizeXS, cfg
                    => cfg.MapFrom(m => m.ShirtSizes.First(x => x.SizeId == 1).Count == 0
                          ? null : m.ShirtSizes.First(x => x.SizeId == 1).Count))
                .ForMember(s => s.SizeS, cfg 
                    => cfg.MapFrom(m => m.ShirtSizes.First(x => x.SizeId == 2).Count == 0
                          ? null : m.ShirtSizes.First(x => x.SizeId == 2).Count))
                .ForMember(s => s.SizeM, cfg
                    => cfg.MapFrom(m => m.ShirtSizes.First(x => x.SizeId == 3).Count == 0
                          ? null : m.ShirtSizes.First(x => x.SizeId == 3).Count))
                .ForMember(s => s.SizeL, cfg 
                    => cfg.MapFrom(m => m.ShirtSizes.First(x => x.SizeId == 4).Count == 0
                          ? null : m.ShirtSizes.First(x => x.SizeId == 4).Count))
                .ForMember(s => s.SizeXL, cfg 
                    => cfg.MapFrom(m => m.ShirtSizes.First(x => x.SizeId == 5).Count == 0
                          ? null : m.ShirtSizes.First(x => x.SizeId == 5).Count))
                .ForMember(s => s.SizeXXL, cfg 
                    => cfg.MapFrom(m => m.ShirtSizes.First(x => x.SizeId == 6).Count == 0
                          ? null : m.ShirtSizes.First(x => x.SizeId == 6).Count));
        }
    }
}