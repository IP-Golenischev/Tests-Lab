﻿using Hw4.DataAccess.Entities.Enums;

namespace Hw4.BusinessLogic.Models;

public class ProductInfo
{
	public int ProductNumber { get; init; }
	public string ProductName { get; init; } = string.Empty;
	public ProductType ProductType { get; init; }
	public int ProductWeight { get; init; }
	public int Price { get; init; }
	public DateTime CreatedAt { get; init; }
	public int StockNumber { get; init; }
}
