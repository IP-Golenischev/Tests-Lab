using HW4.Enums;

namespace HW4.DataTransferObject;

public class CreateProductRequest
{
	public string ProductName { get; init; } = string.Empty;
	public ProductType ProductType { get; init; }
	public int ProductWeight { get; init; }
	public int Price { get; init; }
	public int StockNumber { get; init; }
}
