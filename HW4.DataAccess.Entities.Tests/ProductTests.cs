using FluentAssertions;
using Hw4.DataAccess.Entities;
using Hw4.DataAccess.Entities.Enums;

namespace HW4.DataAccess.Entities.Tests;

public class ProductTests
{
    #region Test data

    public static IEnumerable<object[]> ConstructorData()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
            yield return
            [
                random.Next(1000), new Guid().ToString("N"), random.Next(100), (ProductType)random.Next(5),
                random.Next(1000), DateTime.Now, random.Next(1000)
            ];
    }

    public static IEnumerable<object[]> SimpleProducts()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
            yield return
            [
                new Product(random.Next(1000), new Guid().ToString("N"), random.Next(100), (ProductType)random.Next(5),
                    random.Next(1000), DateTime.Now, random.Next(1000))
            ];
    }

    public static IEnumerable<object[]> TwoSimpleProducts()
    {
        var random = new Random();
        for (var i = 0; i < 10; i++)
            yield return
            [
                new Product(12345, new Guid().ToString("N"), random.Next(100), (ProductType)random.Next(5),
                    random.Next(1000), DateTime.Now, random.Next(1000)),
                new Product(45678, new Guid().ToString("N"), random.Next(100), (ProductType)random.Next(5),
                    random.Next(1000), DateTime.Now, random.Next(1000))
            ];
    }

    public static IEnumerable<object[]> EqualsData()
    {
        var product = new Product(123, "test", 123, ProductType.General, 123, new DateTime(2024, 3, 12), 123);
        return new List<object[]>
        {
            new object[] { product, product },
            new object[]
                { product, new Product(123, "test", 123, ProductType.General, 123, new DateTime(2024, 3, 12), 123) }
        };
    }

    public static IEnumerable<object[]> ProductWithOtherTypesData()
    {
        return new[]
        {
            [new Product(123, "test", 123, ProductType.General, 123, new DateTime(2024, 3, 12), 123), "hello"],
            [new Product(123, "test", 123, ProductType.General, 123, new DateTime(2024, 3, 12), 123), 123],
            [new Product(123, "test", 123, ProductType.General, 123, new DateTime(2024, 3, 12), 123), DateTime.Now],
            new object[] {new Product(123, "test", 123, ProductType.General, 123, new DateTime(2024, 3, 12), 123), new HttpClient()}
        };
    }

    #endregion

    #region Tests

    [Theory]
    [MemberData(nameof(ConstructorData))]
    public void ProductShouldBeCreatedAndValuesShouldBeEquivalent(int productNumber, string productName,
        int productWeight, ProductType productType, int price, DateTime createdAt, int stockNumber)
    {
        var product = new Product(productNumber, productName, productWeight, productType, price, createdAt,
            stockNumber);
        product.ProductName.Should().Be(productName);
        product.ProductNumber.Should().Be(productNumber);
        product.ProductWeight.Should().Be(productWeight);
        product.ProductType.Should().Be(productType);
        product.Price.Should().Be(price);
        product.CreatedAt.Should().Be(createdAt);
        product.StockNumber.Should().Be(stockNumber);
    }

    [Theory]
    [MemberData(nameof(EqualsData))]
    public void ProductsShouldBeEqual(Product expectedProduct, Product actualProduct)
    {
        expectedProduct.Equals(actualProduct).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(TwoSimpleProducts))]
    public void ProductShouldNotBeEqualsById(Product expectedProduct, Product actualProduct)
    {
        expectedProduct.Equals(actualProduct).Should().BeFalse();
    }
    [Theory]
    [MemberData(nameof(ProductWithOtherTypesData))]
    public void ProductShouldNotBeEqualsViaType(Product product, object obj)
    {
        product.Equals(obj).Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(SimpleProducts))]
    public void HashCodeShouldBeCorrect(Product product)
    {
        product.GetHashCode().Should().Be(product.ProductNumber.GetHashCode());
    }

    #endregion
}