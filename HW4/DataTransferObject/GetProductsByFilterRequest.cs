using HW4.Enums;

namespace HW4.DataTransferObject;

public class GetProductsByFilterRequest
{
	public int Skip { get; init; } = 0;
	public int Take { get; init; } = 1;
	public FilterBy FilterBy { get; init; }
	public int StockNumber { get; init; }
	public ProductType ProductType { get; init; }
	public DateTime DateFrom { get; init; }
	public DateTime DateTo { get; init; }
}
