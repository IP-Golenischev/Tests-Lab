using Hw4.DataAccess.Contracts;
using Hw4.DataAccess.Entities;

namespace HW4.DataAccess.Abstractions;

public interface IProductRepository
{
	Product? GetProduct(int productNumber);
	(int itemCount, IReadOnlyCollection<Product> items) GetProducts(ProductsFilterDto options);
	void CreateProduct(Product product);
	void UpdateProduct(Product newProduct);
}
