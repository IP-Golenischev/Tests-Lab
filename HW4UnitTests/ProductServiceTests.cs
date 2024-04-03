using AutoFixture.Xunit2;
using AutoMapper;
using HW4.DataAccess;
using HW4.DataAccess.Abstractions;
using Hw4.DataAccess.Entities;
using HW4.DataAccess.Services;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace HW4UnitTests
{

	public class ProductServiceTests
	{
		private Mock<IProductRepository> _mockRepository;
		private Mock<IMapper> _mockMapper;
		private ProductService _productService;

		public ProductServiceTests()
		{
			_mockRepository = new Mock<IProductRepository>();
			_mockMapper = new Mock<IMapper>();
			_productService = new ProductService(_mockRepository.Object, _mockMapper.Object);
		}

		private void SetupRepositoryWithProduct(Product product)
		{
			var memoryCache = new MemoryCache(new MemoryCacheOptions());
			var cacheService = new MemoryCacheService(memoryCache);
			var repository = new ProductRepository(cacheService);
			repository.CreateProduct(product);

			_mockRepository.Setup(repo => repo.GetProduct(product.ProductNumber)).Returns(product);
		}


		[Fact]
		public void GetProduct_ProductDoesNotExist_ThrowsProductNotFoundException()
		{
			int productNumber = 10;
			_mockRepository.Setup(repo => repo.GetProduct(productNumber)).Returns((Product)null);
			Assert.Throws<ProductNotFoundException>(() => _productService.GetProduct(productNumber));
		}

		[Fact]
		public void Should_Throw_ProductNotFoundException_When_Getting_NonExisting_Product()
		{
			const int nonExistingProductNumber = 999;
			_mockRepository.Setup(repo => repo.GetProduct(nonExistingProductNumber)).Returns((Product)null);
			Assert.Throws<ProductNotFoundException>(() => _productService.GetProduct(nonExistingProductNumber));
		}

		[Fact]
		public void GetProduct_ProductExists_ReturnsProduct()
		{
			int productNumber = 1;
			var expectedProduct = new Product(productNumber, "Cheese", 100, ProductType.Products, 50, DateTime.UtcNow, 10);

			var memoryCache = new MemoryCache(new MemoryCacheOptions());
			var cacheService = new MemoryCacheService(memoryCache);
			var repository = new ProductRepository(cacheService);
			repository.CreateProduct(expectedProduct);

			var actualProduct = repository.GetProduct(productNumber);
			
			ProductServiceTestHelper.AssertProduct(expectedProduct, actualProduct);
		}


		[Fact]
		public void Should_Update_Product_Price_When_Requested()
		{
			const int productNumber = 1;
			const int newPrice = 75;
			var product = new Product(productNumber, "Mango", 100, ProductType.Products, 50, DateTime.UtcNow, 10);
			var mockRepository = new Mock<IProductRepository>();
			var mockMapper = new Mock<IMapper>();
			var productService = new ProductService(mockRepository.Object, mockMapper.Object);

			mockRepository.Setup(repo => repo.GetProduct(productNumber)).Returns(product);

			productService.UpdateProduct(new UpdateProductRequest { ProductNumber = productNumber, Price = newPrice });

			mockRepository.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Once);
		}


		[Fact]
		public void CreateProduct_ValidProduct_CreatesProductSuccessfully()
		{
			var request = CreateValidProductRequest();
			SetupRepositoryForCreateProduct();

			var productId = _productService.CreateProduct(request);

			Assert.True(productId >= 0 && productId <= 10000);
			_mockRepository.Verify(repo => repo.CreateProduct(It.IsAny<Product>()), Times.Once);
		}

		private CreateProductRequest CreateValidProductRequest()
		{
			return new CreateProductRequest
			{
				ProductName = "Car",
				ProductWeight = 100,
				ProductType = ProductType.Technic,
				Price = 50,
				StockNumber = 10
			};
		}

		private void SetupRepositoryForCreateProduct()
		{
			_mockRepository.Setup(repo => repo.CreateProduct(It.IsAny<Product>()));
		}

		[Theory]
		[AutoData]
		public void CreateProductTest(
			string productName, int productWeight, ProductType productType, int price, int stockNumber)
		{
			var product = new CreateProductRequest
			{
				ProductName = productName,
				ProductWeight = productWeight,
				ProductType = productType,
				Price = price,
				StockNumber = stockNumber
			};

			SetupRepositoryForCreateProduct(product);

			_productService.CreateProduct(product);

			VerifyRepositoryWasCalledWithProduct(product);
		}

		private void SetupRepositoryForCreateProduct(CreateProductRequest product)
		{
			_mockRepository.Setup(repo => repo.CreateProduct(It.IsAny<Product>())).Callback<Product>(p =>
			{
				Assert.Equal(product.ProductName, p.ProductName);
				Assert.Equal(product.ProductWeight, p.ProductWeight);
				Assert.Equal(product.ProductType, p.ProductType);
				Assert.Equal(product.Price, p.Price);
				Assert.Equal(product.StockNumber, p.StockNumber);
			});
		}

		private void VerifyRepositoryWasCalledWithProduct(CreateProductRequest expectedProduct)
		{
			_mockRepository.Verify(repo => repo.CreateProduct(It.IsAny<Product>()), Times.Once);
		}


		[Fact]
		public void GetProduct_ReturnsProductInfo()
		{
			int productNumber = 1;
			var expectedProduct = new Product(productNumber, "Watermelon", 100, ProductType.General, 50, DateTime.UtcNow, 10);
			SetupRepositoryForGetProduct(productNumber, expectedProduct);

			var memoryCache = new MemoryCache(new MemoryCacheOptions());
			var cacheService = new MemoryCacheService(memoryCache);
			var repository = new ProductRepository(cacheService);
			repository.CreateProduct(expectedProduct);

			var actualProduct = repository.GetProduct(productNumber);

			ProductServiceTestHelper.AssertProduct(expectedProduct, actualProduct);
		}

		private void SetupRepositoryForGetProduct(int productNumber, Product product)
		{
			_mockRepository.Setup(repo => repo.GetProduct(productNumber)).Returns(product);
		}

		[Fact]
		public void GetProduct_ProductNotFound_ThrowsException()
		{
			var productNumber = 999;
			var mockRepository = new Mock<IProductRepository>();
			var mockMapper = new Mock<IMapper>();
			var productService = new ProductService(mockRepository.Object, mockMapper.Object);

			mockRepository.Setup(repo => repo.GetProduct(productNumber)).Returns((Product)null);

			Assert.Throws<ProductNotFoundException>(() => ((IProductService)productService).GetProduct(productNumber));
		}


		[Theory, AutoData]
		public void UpdateProductTest(UpdateProductRequest request)
		{
			var productNumber = request.ProductNumber;
			var product = new Product(productNumber, "Mandarin", 100, ProductType.General, 50, DateTime.UtcNow, 10);
			var mockRepository = new Mock<IProductRepository>();
			mockRepository.Setup(repo => repo.GetProduct(productNumber)).Returns(product);
			var mockMapper = new Mock<IMapper>();
			var productService = new ProductService(mockRepository.Object, mockMapper.Object);

			productService.UpdateProduct(request);

			mockRepository.Verify(repo => repo.UpdateProduct(It.IsAny<Product>()), Times.Once);
			Assert.Equal(request.Price, product.Price);
		}



		[Fact]
		public void GetProducts_ReturnsListOfProducts()
		{
			var memoryCache = new MemoryCache(new MemoryCacheOptions());
			var cacheService = new MemoryCacheService(memoryCache);
			var repository = new ProductRepository(cacheService);

			var products = new[]
			{
				new Product(1, "Cheese", 100, ProductType.Products, 50, DateTime.UtcNow, 10),
				new Product(2, "Milk", 200, ProductType.Products, 60, DateTime.UtcNow, 20),
				new Product(3, "Bread", 150, ProductType.Products, 70, DateTime.UtcNow, 30),
				new Product(4, "Eggs", 120, ProductType.Products, 80, DateTime.UtcNow, 40),
				new Product(5, "Butter", 180, ProductType.Products, 90, DateTime.UtcNow, 50)
			};

			foreach (var product in products)
			{
				repository.CreateProduct(product);
			}

			foreach (var product in products)
			{
				var result = repository.GetProduct(product.ProductNumber);

				Assert.NotNull(result);
				Assert.Equal(product.ProductNumber, result.ProductNumber);
				Assert.Equal(product.ProductName, result.ProductName);
				Assert.Equal(product.ProductWeight, result.ProductWeight);
				Assert.Equal(product.ProductType, result.ProductType);
				Assert.Equal(product.Price, result.Price);
				Assert.Equal(product.CreatedAt, result.CreatedAt);
				Assert.Equal(product.StockNumber, result.StockNumber);
			}
		}

		[Fact]
		public void GetProducts_ReturnsPaginationResponse()
		{
			var products = new List<Product>
			{
				new Product(1, "Water", 100, ProductType.General, 50, DateTime.UtcNow, 10),
				new Product(2, "Soda", 150, ProductType.HouseholdChemicals, 60, DateTime.UtcNow, 15),
				new Product(3, "Computer", 200, ProductType.Technic, 70, DateTime.UtcNow, 20)
			};

			var repository = SetupRepositoryWithProducts(products);

			AssertProducts(repository, products);
		}

		private IProductRepository SetupRepositoryWithProducts(List<Product> products)
		{
			var memoryCache = new MemoryCache(new MemoryCacheOptions());
			var cacheService = new MemoryCacheService(memoryCache);
			var repository = new ProductRepository(cacheService);

			foreach (var product in products)
			{
				repository.CreateProduct(product);
			}

			return repository;
		}

		private void AssertProducts(IProductRepository repository, List<Product> expectedProducts)
		{
			foreach (var expectedProduct in expectedProducts)
			{
				var result = repository.GetProduct(expectedProduct.ProductNumber);

				Assert.Equal(expectedProduct.ProductNumber, result.ProductNumber);
				Assert.Equal(expectedProduct.ProductName, result.ProductName);
				Assert.Equal(expectedProduct.ProductType, result.ProductType);
				Assert.Equal(expectedProduct.ProductWeight, result.ProductWeight);
				Assert.Equal(expectedProduct.Price, result.Price);
				Assert.Equal(expectedProduct.CreatedAt, result.CreatedAt);
				Assert.Equal(expectedProduct.StockNumber, result.StockNumber);
			}
		}
	}
}