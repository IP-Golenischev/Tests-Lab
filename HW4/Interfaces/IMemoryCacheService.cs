namespace HW4.Interfaces;

public interface IMemoryCacheService
{
	T GetOrCreate<T>(string key, T obj);
}
