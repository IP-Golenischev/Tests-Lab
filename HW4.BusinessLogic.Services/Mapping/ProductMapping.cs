using AutoMapper;
using Hw4.BusinessLogic.Models;
using Hw4.DataAccess.Entities;

namespace HW4.BusinessLogic.Services.Mapping;

public class ProductMapping : Profile
{
	public ProductMapping()
	{
		CreateMap<Product, ProductInfo>().ReverseMap();
	}
}
