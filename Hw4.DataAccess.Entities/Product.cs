using Hw4.DataAccess.Entities.Enums;

namespace Hw4.DataAccess.Entities;

public class Product(
	int productNumber,
	string productName,
	int productWeight,
	ProductType productType,
	int price,
	DateTime createdAt,
	int stockNumber)
{
	public int ProductNumber { get; init; } = productNumber;
	public string ProductName { get; init; } = productName;
	public int ProductWeight { get; init; } = productWeight;
	public ProductType ProductType { get; init; } = productType;
	public int Price { get; set; } = price;
	public DateTime CreatedAt { get; init; } = createdAt;
	public int StockNumber { get; init; } = stockNumber;

	public override bool Equals(object? obj)
	{
		if (obj is not Product product)
		{
			return false;
		}

		return product.ProductNumber == ProductNumber;
	}

	public override int GetHashCode()
	{
		return ProductNumber.GetHashCode();
	}
}
