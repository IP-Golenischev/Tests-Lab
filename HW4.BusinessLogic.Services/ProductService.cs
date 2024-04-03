using AutoMapper;
using Hw4.BusinessLogic.Models;
using HW4.BusinessLogic.Services.Base;
using HW4.BusinessLogic.Services.DataTransferObjects;
using HW4.BusinessLogic.Services.Exceptions;
using HW4.Common.Helpers;
using HW4.DataAccess.Abstractions;
using Hw4.DataAccess.Contracts;
using Hw4.DataAccess.Entities;

namespace HW4.BusinessLogic.Services
{
	public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
	{
		private readonly IProductRepository _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
		private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		private const int MaxProductNumber = 10000;

		public ProductInfo? GetProduct(int productNumber)
		{
			var product = _productRepository.GetProduct(productNumber) ?? throw new ProductNotFoundException(ExceptionMessages.ProductNotFoundException);
			return _mapper.Map<ProductInfo>(product);
		}

		public PaginationResponse<ProductInfo> GetProducts(GetProductsByFilterRequest options)
		{
			var products = _productRepository.GetProducts(_mapper.Map<ProductsFilterDto>(options));
			var productsInfo = _mapper.Map<ProductInfo[]>(products.items);
			return new PaginationResponse<ProductInfo>
			{
				ItemCount = products.itemCount,
				Items = productsInfo
			};
		}

		public int CreateProduct(CreateProductRequest request)
		{
			var number = new Random().Next(0, MaxProductNumber);
			var product = new Product(
				number,
				request.ProductName,
				request.ProductWeight,
				request.ProductType,
				request.Price,
				DateTime.Now.ToUniversalTime(),
				request.StockNumber
			);
			_productRepository.CreateProduct(product);
			return number;
		}

		public void UpdateProduct(UpdateProductRequest newProduct)
		{
			var product = _productRepository.GetProduct(newProduct.ProductNumber);
			product.Price = newProduct.Price;
			_productRepository.UpdateProduct(product);

		}
	}
}
