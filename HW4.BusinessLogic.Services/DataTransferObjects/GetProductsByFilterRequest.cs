using Hw4.DataAccess.Contracts.Enums;
using Hw4.DataAccess.Entities.Enums;

namespace HW4.BusinessLogic.Services.DataTransferObjects;

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
