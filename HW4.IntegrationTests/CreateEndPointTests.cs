using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using HW4.BusinessLogic.Services.DataTransferObjects;
using Hw4.DataAccess.Entities;
using Hw4.DataAccess.Entities.Enums;
using HW4.IntegrationTests.Helpers;

namespace HW4.IntegrationTests;

public class CreateEndPointTests
{
    public static IEnumerable<object[]> TestsData()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
            yield return
            [
                new CreateProductRequest()
                {
                    ProductType =  (ProductType)random.Next(5),
                    ProductWeight = random.Next(1000),
                    StockNumber = random.Next(1000),
                    Price = random.Next(1000),
                    ProductName = Guid.NewGuid().ToString("N")
                }
            ];
    }
    private readonly WebHostFactory _factory = new();
    private readonly HttpClient _client;
    public CreateEndPointTests()
    {
        _client = _factory.CreateClient();
    }
    [Theory]
    [MemberData(nameof(TestsData))]
    public async Task ProductShouldBeCreated(CreateProductRequest request)
    {
        var response = await _client.PostAsJsonAsync("/api/Product/Create",request);
        response.IsSuccessStatusCode.Should().BeTrue();
        var body = await response.Content.ReadFromJsonAsync<int>();
        body.Should().BePositive();
    }
    [Fact]
    public async Task ProductShouldNotBeCreatedViaEmptyName()
    {
        var random = new Random();
        var product = new Product(random.Next(1000), "", random.Next(100), (ProductType)random.Next(5),
            random.Next(1000), DateTime.Now, random.Next(1000));
        var response = await _client.PostAsJsonAsync("/api/Product/Create",product);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
    }
    [Fact]
    public async Task ProductShouldNotBeCreatedZeroProductWeight()
    {
        var random = new Random();
        var product = new Product(random.Next(1000), "", 0, (ProductType)random.Next(5),
            random.Next(1000), DateTime.Now, random.Next(1000));
        var response = await _client.PostAsJsonAsync("/api/Product/Create",product);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
    }
    [Fact]
    public async Task ProductShouldNotBeCreatedZeroProductPrice()
    {
        var random = new Random();
        var product = new Product(random.Next(1000), "", random.Next(1000), (ProductType)random.Next(5),
            0, DateTime.Now, random.Next(1000));
        var response = await _client.PostAsJsonAsync("/api/Product/Create",product);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
    }
    [Fact]
    public async Task ProductShouldNotBeCreatedZeroProductStock()
    {
        var random = new Random();
        var product = new Product(random.Next(1000), "", random.Next(1000), (ProductType)random.Next(5),
            random.Next(1000), DateTime.Now, 0);
        var response = await _client.PostAsJsonAsync("/api/Product/Create",product);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        
    }
}