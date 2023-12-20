using AutoMapper;

namespace Basket.Application.Mappers;
public static class BasketMapper
{
    public static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
            cfg.AddProfile<BasketMappingProfile>();
        });

        return configuration.CreateMapper();
    });

    public static IMapper Mapper => Lazy.Value;
}