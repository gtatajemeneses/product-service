using AutoMapper;
using ProductService.Dtos;
using ProductService.Entities;

namespace ProductService.Profiles;
public class ProductProfile:Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductRequestDto>().ReverseMap();
        CreateMap<Product, ProductResponseDto>().ReverseMap();
    }        
}
