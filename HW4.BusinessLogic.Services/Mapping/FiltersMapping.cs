using AutoMapper;
using HW4.BusinessLogic.Services.DataTransferObjects;
using Hw4.DataAccess.Contracts;

namespace HW4.BusinessLogic.Services.Mapping;

public class FiltersMapping : Profile
{
    public FiltersMapping()
    {
        CreateMap<GetProductsByFilterRequest, ProductsFilterDto>();
    }
}