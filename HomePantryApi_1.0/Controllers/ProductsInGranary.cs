using HomePantryApi_1._0.Models;
using HomePantryApi_1._0.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace HomePantryApi_1._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsInGranaryController : ControllerBase
{
    private readonly IProductsInGranaryRepository _productsInGranaryRepository;

    public ProductsInGranaryController(IProductsInGranaryRepository productsInGranaryRepository)
    {
        _productsInGranaryRepository = productsInGranaryRepository;
    }

    [HttpGet("{granaryId}")]
    public async Task<IActionResult> GetAllProductsInGranary(int granaryId)
    {
        var products = await _productsInGranaryRepository.GetProductsByGranaryIdAsync(granaryId);
        return Ok(products);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProductInGranaryById(int productId)
    {
        var product = await _productsInGranaryRepository.GetProductByIdAsync(productId);
        if (product == null)
        {
            return NotFound("Product not found in granary.");
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToGranary(Productsingranary product)
    {
        await _productsInGranaryRepository.AddProductToGranaryAsync(product);
        return CreatedAtAction(nameof(GetProductInGranaryById), new { productId = product.ProductId }, product);
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProductInGranary(int productId, Productsingranary product)
    {
        if (productId != product.ProductId)
        {
            return BadRequest("Product ID mismatch.");
        }

        var existingProduct = await _productsInGranaryRepository.GetProductsByGranaryIdAsync(productId);
        if (existingProduct == null)
        {
            return NotFound("Product not found in granary.");
        }

        await _productsInGranaryRepository.UpdateProductInGranaryAsync(product);
        return NoContent();
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProductFromGranary(int productId)
    {
        var product = await _productsInGranaryRepository.GetProductByIdAsync(productId);
        if (product == null)
        {
            return NotFound("Product not found in granary.");
        }

        await _productsInGranaryRepository.DeleteProductFromGranaryAsync(productId);
        return NoContent();
    }
}
