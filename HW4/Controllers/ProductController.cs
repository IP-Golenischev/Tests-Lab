using HW4.BusinessLogic.Services.Base;
using HW4.BusinessLogic.Services.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace HW4.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
	[HttpGet("List")]
	public IActionResult GetProducts([FromQuery] GetProductsByFilterRequest request)
	{
		var product = productService.GetProducts(request);
		return Ok(product);
	}

	[HttpGet("{productNumber:int}")]
	public IActionResult Get([FromRoute] int productNumber)
	{
		var product = productService.GetProduct(productNumber);
		return Ok(product);
	}

	[HttpPost("Create")]
	public IActionResult Create([FromBody] CreateProductRequest request)
	{
		var number = productService.CreateProduct(request);
		return Ok(number);
	}

	[HttpPut("Update")]
	public IActionResult Update([FromBody] UpdateProductRequest request)
	{
		productService.UpdateProduct(request);
		return NoContent();
	}
}
