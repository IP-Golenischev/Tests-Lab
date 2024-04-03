namespace HW4.DataAccess.Abstractions;

public interface IMemoryCacheService
{
	T GetOrCreate<T>(string key, T obj);
}
