using AutoMapper;
using WebAppHero.Contract.Services.V1.Product;
using WebAppHero.Domain.Entities;

namespace WebAppHero.Application.Mappers;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Version 1
        CreateMap<Product, Response.ProductResponse>().ReverseMap();
    }
}
