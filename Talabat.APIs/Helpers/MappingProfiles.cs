using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Order_Aggregate;

//using Talabat.Core.Entities.Identity;
using static System.Net.WebRequestMethods;

namespace Talabat.APIs.Helpers
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(distnation => distnation.Brand, options => options.MapFrom(source => source.Brand.Name))
                .ForMember(distnation => distnation.Category, options => options.MapFrom(source => source.Category.Name))
                //.ForMember(distnation => distnation.PictureUrl, options => options.MapFrom(source => $"{"https://localhost:7264"}/{source.PictureUrl}"));
                .ForMember(distnation => distnation.PictureUrl, options => options.MapFrom<ProductPictureUrlResolver>());


            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();

            //CreateMap<Address, AddressDto>().ReverseMap();

            CreateMap<AddressDto, Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, options => options.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, options => options.MapFrom(s => s.DeliveryMethod.Cost));


            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureResolver>());
		}
    }
}
