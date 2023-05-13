using AutoMapper;
using ProductApi.DTOs;
using ProductApi.Models;

namespace ProductApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ProductDetails, Product>();
            CreateMap<USP_GetProductsResult, ProductDetails>();
        }
    }
    
}
