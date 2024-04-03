using HW4.Models;
using HW4.DataTransferObject;

namespace HW4.Interfaces;

public interface IProductRepository
{
	Product? GetProduct(int productNumber);
	(int itemCount, IReadOnlyCollection<Product> items) GetProducts(GetProductsByFilterRequest options);
	void CreateProduct(Product product);
	void UpdateProduct(Product newProduct);
}
