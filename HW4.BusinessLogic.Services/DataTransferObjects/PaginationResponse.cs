namespace HW4.BusinessLogic.Services.DataTransferObjects;

public class PaginationResponse<T>
{
	public int ItemCount { get; init; }
	public T[] Items { get; init; } = Array.Empty<T>();
}
