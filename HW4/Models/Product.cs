using HW4.Enums;

namespace HW4.Models;

public class Product
{
	public int ProductNumber { get; set; }
	public string ProductName { get; set; }
	public int ProductWeight { get; set; }
	public ProductType ProductType { get; set; }
	public int Price { get; set; }
	public DateTime CreatedAt { get; set; }
	public int StockNumber { get; set; }

	public Product(int productNumber, string productName, int productWeight, ProductType productType, int price, DateTime createdAt, int stockNumber)
	{
		ProductNumber = productNumber;
		ProductName = productName;
		ProductWeight = productWeight;
		ProductType = productType;
		Price = price;
		CreatedAt = createdAt;
		StockNumber = stockNumber;
	}

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
