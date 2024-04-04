using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Hw4.BusinessLogic.Models;
using HW4.BusinessLogic.Services.DataTransferObjects;
using Hw4.DataAccess.Entities.Enums;
using HW4.IntegrationTests.Helpers;

namespace HW4.IntegrationTests;

public class UpdateProductEndPointTests
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
    public UpdateProductEndPointTests()
    {
        _client = _factory.CreateClient();
    }
    [Theory]
    [MemberData(nameof(TestsData))]
    public async Task ProductShouldBeCreated(CreateProductRequest request)
    {
        var response = await _client.PostAsJsonAsync("/api/Product/Create",request);
        var productNumber = await response.Content.ReadFromJsonAsync<int>();
        var updateProductRequest = new UpdateProductRequest()
        {
            ProductNumber = productNumber,
            Price = new Random().Next(1000)
        };
        var result = await _client.PutAsJsonAsync("/api/Product/Update", updateProductRequest);
        result.IsSuccessStatusCode.Should().BeTrue();
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var updatedProduct = await _client.GetFromJsonAsync<ProductInfo>($"api/Product/{productNumber}", options);
        updatedProduct.Should().NotBeNull();
        updatedProduct?.ProductNumber.Should().Be(productNumber);
        updatedProduct?.Price.Should().Be(updateProductRequest.Price);

    }
}