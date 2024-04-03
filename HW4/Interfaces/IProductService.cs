using HW4.DataTransferObject;

namespace HW4.Interfaces;

public interface IProductService
{
	HW4.DataTransferObject.ProductInfo? GetProduct(int productNumber);
	PaginationResponse<HW4.DataTransferObject.ProductInfo> GetProducts(GetProductsByFilterRequest request);
	int CreateProduct(CreateProductRequest request);
	void UpdateProduct(UpdateProductRequest newProduct);
}
