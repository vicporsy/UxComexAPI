using Vicporsy.Domain.Models;
using Vicporsy.Contract.Response;
using AutoMapper;
using Vicporsy.Contract.Request;

namespace Vicporsy.Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Client, ClientResponse>().ReverseMap();
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<Order, OrderResponse>().ReverseMap();
            CreateMap<OrderItem, OrderItemResponse>().ReverseMap();
        }
    }
}
