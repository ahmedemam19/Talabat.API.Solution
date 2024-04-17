using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;

namespace Talabat.APIs.Helpers
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(distnation => distnation.Brand, options => options.MapFrom(source => source.Brand.Name))
                .ForMember(distnation => distnation.Category, options => options.MapFrom(source => source.Category.Name));

		}
    }
}
