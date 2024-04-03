using AutoMapper;
using HW4.Models;
using HW4.DataTransferObject;

namespace HW4.Mappers;

public class ProductMapping : Profile
{
	public ProductMapping()
	{
		CreateMap<Product, HW4.DataTransferObject.ProductInfo>().ReverseMap();
	}
}
