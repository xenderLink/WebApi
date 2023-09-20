using AutoMapper;
using Store.Models.DTO;
using Store.Models.Entities;

namespace Store.Profiles;

public sealed class SubCategoryProfile : Profile
{
    public SubCategoryProfile()
    {
        CreateMap<SubCategory, SubCatDto>()
            .ForMember(c => c.Id, opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(c => c.Name, opt => opt.MapFrom(src => src.Name));

        CreateMap<CrtSubCatDto, SubCategory>()
            .ForMember(c => c.Name, opt => opt.MapFrom(src => src.Name));
    }
}