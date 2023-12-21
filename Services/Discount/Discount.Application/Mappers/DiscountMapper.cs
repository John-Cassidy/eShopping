using AutoMapper;

namespace Discount.Application.Mappers;

public static class DiscountMapper
{
    public static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
            cfg.AddProfile<DiscountMappingProfile>();
        });

        return configuration.CreateMapper();
    });

    public static IMapper Mapper => Lazy.Value;
}
