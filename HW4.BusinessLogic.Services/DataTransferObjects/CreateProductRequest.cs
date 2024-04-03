using Hw4.DataAccess.Entities.Enums;

namespace HW4.BusinessLogic.Services.DataTransferObjects;

public class CreateProductRequest
{
	public string ProductName { get; init; } = string.Empty;
	public ProductType ProductType { get; init; }
	public int ProductWeight { get; init; }
	public int Price { get; init; }
	public int StockNumber { get; init; }
}
