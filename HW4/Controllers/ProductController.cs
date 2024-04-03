using Microsoft.AspNetCore.Mvc;
using HW4.DataTransferObject;
using HW4.Interfaces;

namespace HW4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
	private readonly IProductService _productService;

	public ProductController(IProductService productService)
	{
		_productService = productService;
	}

	[HttpGet("List")]
	public IActionResult GetProducts([FromQuery] GetProductsByFilterRequest request)
	{
		var product = _productService.GetProducts(request);
		return Ok(product);
	}

	[HttpGet("{productNumber:int}")]
	public IActionResult Get([FromRoute] int productNumber)
	{
		var product = _productService.GetProduct(productNumber);
		return Ok(product);
	}

	[HttpPost("Create")]
	public IActionResult Create([FromBody] CreateProductRequest request)
	{
		var number = _productService.CreateProduct(request);
		return Ok(number);
	}

	[HttpPut("Update")]
	public IActionResult Update([FromBody] UpdateProductRequest request)
	{
		_productService.UpdateProduct(request);
		return NoContent();
	}
}
