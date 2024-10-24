using HomePantryApi_1._0.Models;
using HomePantryApi_1._0.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HomePantryApi_1._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsInShoplistController : ControllerBase
{
    private readonly IProductsInShoplistRepository _productsInShoplistRepository;

    public ProductsInShoplistController(IProductsInShoplistRepository productsInShoplistRepository)
    {
        _productsInShoplistRepository = productsInShoplistRepository;
    }

    [HttpGet("{shoplistId}")]
    public async Task<IActionResult> GetAllProductsInShoplist(int shoplistId)
    {
        var products = await _productsInShoplistRepository.GetProductsByShoplistIdAsync(shoplistId);
        return Ok(products);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetProductInShoplistById(int productId)
    {
        var product = await _productsInShoplistRepository.GetProductByIdAsync(productId);
        if (product == null)
        {
            return NotFound("Product not found in shoplist.");
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> AddProductToShoplist(Productsinshoplist product)
    {
        await _productsInShoplistRepository.AddProductToShoplistAsync(product);
        return CreatedAtAction(nameof(GetProductInShoplistById), new { productId = product.ProductId }, product);
    }

    [HttpPut("{productId}")]
    public async Task<IActionResult> UpdateProductInShoplist(int productId, Productsinshoplist product)
    {
        if (productId != product.ProductId)
        {
            return BadRequest("Product ID mismatch.");
        }

        var existingProduct = await _productsInShoplistRepository.GetProductByIdAsync(productId);
        if (existingProduct == null)
        {
            return NotFound("Product not found in shoplist.");
        }

        await _productsInShoplistRepository.UpdateProductInShoplistAsync(product);
        return NoContent();
    }

    [HttpDelete("{productId}")]
    public async Task<IActionResult> DeleteProductFromShoplist(int productId)
    {
        var product = await _productsInShoplistRepository.GetProductByIdAsync(productId);
        if (product == null)
        {
            return NotFound("Product not found in shoplist.");
        }

        await _productsInShoplistRepository.DeleteProductFromShoplistAsync(productId);
        return NoContent();
    }
}

