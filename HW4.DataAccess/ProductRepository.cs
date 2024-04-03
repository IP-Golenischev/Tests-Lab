using HW4.Common.Helpers;
using HW4.DataAccess.Abstractions;
using Hw4.DataAccess.Contracts;
using Hw4.DataAccess.Contracts.Enums;
using Hw4.DataAccess.Entities;

namespace HW4.DataAccess;

public class ProductRepository(IMemoryCacheService cacheService) : IProductRepository
{
	private const string ProductCacheKey = "products";
	private readonly object _writeLock = new();

	public Product? GetProduct(int productNumber)
	{
		var products = cacheService.GetOrCreate(ProductCacheKey, new HashSet<Product>());
		return products.FirstOrDefault(c => c.ProductNumber == productNumber);
	}

	public (int itemCount, IReadOnlyCollection<Product> items) GetProducts(ProductsFilterDto options)
	{
		var products = cacheService.GetOrCreate(ProductCacheKey, new HashSet<Product>());

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
			var products = cacheService.GetOrCreate(ProductCacheKey, new HashSet<Product>());
			products.Add(product);
		}
	}

	public void UpdateProduct(Product newProduct)
	{
		lock (_writeLock)
		{
			var products = cacheService.GetOrCreate(ProductCacheKey, new HashSet<Product>());
			products.Remove(newProduct);
			products.Add(newProduct);
		}
	}
}
