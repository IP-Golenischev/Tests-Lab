using Microsoft.Extensions.Caching.Memory;
using HW4.Interfaces;

namespace HW4.Services;

public class MemoryCacheService : IMemoryCacheService
{
	private IMemoryCache Cache { get; init; }

	public MemoryCacheService(IMemoryCache cache)
	{
		Cache = cache;
	}

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
