using AutoMapper;
using HW4.DataTransferObject;
using HW4.Exceptions;
using HW4.Interfaces;
using HW4.Models;
using HW4.Primitives;

namespace HW4.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;
		private readonly IMapper _mapper;
		private const int MaxProductNumber = 10000;

		public ProductService(IProductRepository productRepository, IMapper mapper)
		{
			_productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		}

		public HW4.DataTransferObject.ProductInfo? GetProduct(int productNumber)
		{
			var product = _productRepository.GetProduct(productNumber) ?? throw new ProductNotFoundException(ExceptionMessages.ProductNotFoundException);
			return _mapper.Map<HW4.DataTransferObject.ProductInfo>(product);
		}

		public PaginationResponse<HW4.DataTransferObject.ProductInfo> GetProducts(GetProductsByFilterRequest options)
		{
			var products = _productRepository.GetProducts(options);
			var productsInfo = _mapper.Map<HW4.DataTransferObject.ProductInfo[]>(products.items);
			return new PaginationResponse<HW4.DataTransferObject.ProductInfo>
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
