using HW4.DataAccess.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace HW4.DataAccess.Services;

public class MemoryCacheService(IMemoryCache cache) : IMemoryCacheService
{
	private IMemoryCache Cache { get; init; } = cache;

	public T GetOrCreate<T>(string key, T obj)
	{
		var cacheData = Cache.Get<T>(key);

		if (cacheData is null)
		{
			Cache.Set(key, obj);
		}

		return Cache.Get<T>(key) ?? obj;
	}
}
