using HW4.DataTransferObject;
using HW4.Interfaces;
using HW4.Models;
using HW4.Primitives;
using HW4.Enums;

namespace HW4.Repositories;

public class ProductRepository : IProductRepository
{
	private readonly IMemoryCacheService _cacheService;
	private const string ProductCacheKey = "products";
	private readonly object _writeLock = new();

	public ProductRepository(IMemoryCacheService cacheService)
	{
		_cacheService = cacheService;
	}

	public Product? GetProduct(int productNumber)
	{
		var products = _cacheService.GetOrCreate(ProductCacheKey, new HashSet<Product>());
		return products.FirstOrDefault(c => c.ProductNumber == productNumber);
	}

	public (int itemCount, IReadOnlyCollection<Product> items) GetProducts(GetProductsByFilterRequest options)
	{
		var products = _cacheService.GetOrCreate(ProductCacheKey, new HashSet<Product>());

		var filteredProducts = options.FilterBy switch
		{
			FilterBy.Undefined => products,
			FilterBy.CreatedDate => products.Where(c => c.CreatedAt >= options.DateFrom && c.CreatedAt <= options.DateTo),
			FilterBy.ProductType => products.Where(c => c.ProductType == options.ProductType),
			FilterBy.Stock => products.Where(c => c.StockNumber == options.StockNumber),
			_ => throw new ArgumentOutOfRangeException(ExceptionMessages.FilterNotFoundException)
		};
		var filteredProductsPaginated = filteredProducts.Skip(options.Skip).Take(options.Take).ToArray();
		return (products.Count, filteredProductsPaginated.AsReadOnly());
	}

	public void CreateProduct(Product product)
	{
		lock (_writeLock)
		{
			var products = _cacheService.GetOrCreate(ProductCacheKey, new HashSet<Product>());
			products.Add(product);
		}
	}

	public void UpdateProduct(Product newProduct)
	{
		lock (_writeLock)
		{
			var products = _cacheService.GetOrCreate(ProductCacheKey, new HashSet<Product>());

			products.Remove(newProduct);
			products.Add(newProduct);
		}
	}
}
