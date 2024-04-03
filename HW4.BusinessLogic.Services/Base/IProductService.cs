using Hw4.BusinessLogic.Models;
using HW4.BusinessLogic.Services.DataTransferObjects;

namespace HW4.BusinessLogic.Services.Base;

public interface IProductService
{
	ProductInfo? GetProduct(int productNumber);
	PaginationResponse<ProductInfo> GetProducts(GetProductsByFilterRequest request);
	int CreateProduct(CreateProductRequest request);
	void UpdateProduct(UpdateProductRequest newProduct);
}
