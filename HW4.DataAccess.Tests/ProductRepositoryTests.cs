using FluentAssertions;
using HW4.DataAccess.Abstractions;
using Hw4.DataAccess.Entities;
using Hw4.DataAccess.Entities.Enums;
using Moq;

namespace HW4.DataAccess.Tests;

public class ProductRepositoryTests
{
    private readonly Mock<IMemoryCacheService> _memoryCacheServiceMock = new();
    private readonly IProductRepository _productRepository;

    public ProductRepositoryTests()
    {
        _productRepository = new ProductRepository(_memoryCacheServiceMock.Object);
    }
    [Fact]
    void GetProductWhenCacheIsNull()
    {
        _memoryCacheServiceMock.Setup(service => service.GetOrCreate(It.IsAny<string>(), It.IsAny<object>()))
            .Returns(new HashSet<Product>());
        var result = _productRepository.GetProduct(12345);
        result.Should().BeNull();
    }
    [Fact]
    void GetProductWhenCacheIsEmpty()
    {
        var prodocts = new HashSet<Product>();
        _memoryCacheServiceMock.Setup(service => service.GetOrCreate(It.IsAny<string>(), It.IsAny<object>()))
            .Returns(prodocts);
        var result = _productRepository.GetProduct(12345);
        result.Should().BeNull();
    }
    [Fact]
    void GetProductWhenProductIsAbsent()
    {
        var products = new HashSet<Product> { new Product(6789, "test",123,ProductType.General,123, DateTime.Now, 123)};
        _memoryCacheServiceMock.Setup(service => service.GetOrCreate(It.IsAny<string>(), It.IsAny<object>()))
            .Returns(products);
        var result = _productRepository.GetProduct(12345);
        result.Should().BeNull();
    }
    [Fact]
    void ProductShouldBeFindExactly()
    {
        var products = new HashSet<Product> { new(12345, "test",123,ProductType.General,123, DateTime.Now, 123)};
        _memoryCacheServiceMock.Setup(service => service.GetOrCreate(It.IsAny<string>(), It.IsAny<object>()))
            .Returns(products);
        var result = _productRepository.GetProduct(12345);
        result.Should().NotBeNull();
        result.ProductNumber.Should().BeOneOf(12345);
    }
    [Fact]
    void ProductShouldBeCreated()
    {
        var products = new HashSet<Product> { new(6789, "test",123,ProductType.General,123, DateTime.Now, 123)};
        _memoryCacheServiceMock.Setup(service => service.GetOrCreate(It.IsAny<string>(), It.IsAny<object>()))
            .Returns(products);
        var product = new Product(12345, "test", 123, ProductType.General, 123, DateTime.Now, 123);
        _productRepository.CreateProduct(product);
        var addedProduct = _productRepository.GetProduct(12345);
        addedProduct.Should().BeEquivalentTo(product);

    }
    [Fact]
    void ProductShouldBeUpdated()
    {
        var products = new HashSet<Product> { new(12345, "hello",456,ProductType.HouseholdChemicals,456, DateTime.Now, 456)};
        _memoryCacheServiceMock.Setup(service => service.GetOrCreate(It.IsAny<string>(), It.IsAny<object>()))
            .Returns(products);
        var product = new Product(12345, "test", 123, ProductType.General, 123, DateTime.Now, 123);
        _productRepository.UpdateProduct(product);
        var updatedProduct = _productRepository.GetProduct(12345);
        updatedProduct.Should().BeEquivalentTo(product);
        updatedProduct.ProductName.Should().BeEquivalentTo(product.ProductName);
        updatedProduct.ProductNumber.Should().Be(product.ProductNumber);
        updatedProduct.ProductType.Should().Be(product.ProductType);
        updatedProduct.ProductWeight.Should().Be(product.ProductWeight);
        updatedProduct.CreatedAt.Should().Be(product.CreatedAt);
        updatedProduct.Price.Should().Be(product.Price);
        updatedProduct.StockNumber.Should().Be(product.StockNumber);

    }
    
}