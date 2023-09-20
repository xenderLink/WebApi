using AutoMapper;
using Store.Models.DTO;
using Store.Models.Entities;

namespace Store.Profiles;

public sealed class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(p => p.Id, opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(p => p.Name, opt => opt.MapFrom(src => src.Name)
            )
            .ForMember(p => p.Description, opt => opt.MapFrom(src => src.Description)
            )
            .ForMember(p => p.Sku, opt => opt.MapFrom(src => src.Sku));

        CreateMap<CrtProductDto, Product>()
            .ForMember(p => p.Name, opt => opt.MapFrom(src => src.Name)
            )
            .ForMember(p => p.Price, opt => opt.MapFrom(src => src.Price)
            )
            .ForMember(p => p.Sku, opt => opt.MapFrom(src => src.Sku)
            )
            .ForMember(p => p.Description, opt => opt.MapFrom(src => src.Description));
    }
}