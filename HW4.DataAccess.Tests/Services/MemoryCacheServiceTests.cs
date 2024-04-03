using FluentAssertions;
using Hw4.DataAccess.Entities;
using HW4.DataAccess.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace HW4.DataAccess.Tests.Services;

public class MemoryCacheServiceTests
{
    private readonly Mock<IMemoryCache> _cachingMock = new();
    private readonly MemoryCacheService _memoryCacheService;

    public static IEnumerable<object[]> TestData()
    {
        return new[]
        {
            ["key", "hello"],
            ["key", 123],
            ["key", new HttpClient()],
            new Object[]{"key", DateTime.Now },
        };
    }
    public MemoryCacheServiceTests()
    {
        _memoryCacheService = new MemoryCacheService(_cachingMock.Object);
    }
    [Theory]
    [MemberData(nameof(TestData))]
    public void ObjectShouldBeCreatedWhenAbsent(string key, object value)
    {
        object? res= null;
        _cachingMock.Setup(cache => cache.TryGetValue(key,out res)).Returns((string p, ref object? q) =>
        {
            q = null;
            return false;
        });
        _cachingMock.Setup(cache => cache.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>()).Verifiable();
        var result = _memoryCacheService.GetOrCreate(key, value);
        result.Should().BeEquivalentTo(value);
    }
    [Theory]
    [MemberData(nameof(TestData))]
    public void ObjectShouldBeCreatedWhenPresent(string key, object value)
    {
        object? res= value;
        _cachingMock.Setup(cache => cache.TryGetValue(key,out res)).Returns((string p, ref object? q) =>
        {
            q = value;
            return true;
        });
        _cachingMock.Verify(cache => cache.CreateEntry(It.IsAny<object>()), Times.Never);
        var result = _memoryCacheService.GetOrCreate(key, value);
        result.Should().BeEquivalentTo(value);
    }
}