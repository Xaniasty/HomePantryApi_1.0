using HomePantryApi_1._0.Models;
using HomePantryApi_1._0.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace HomePantryApi_1._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShoplistController : ControllerBase
{
    private readonly IShoplistRepository _shoplistRepository;
    private readonly IProductsInShoplistRepository _productsInShoplistRepository;

    public ShoplistController(IShoplistRepository shoplistRepository, IProductsInShoplistRepository productsInShoplistRepository)
    {
        _shoplistRepository = shoplistRepository;
        _productsInShoplistRepository = productsInShoplistRepository;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAllShoplistsForUser(int userId)
    {
        var shoplists = await _shoplistRepository.GetAllShopListsForUserAsync(userId);
        return Ok(shoplists);
    }

    [HttpGet("shoplist/{shoplistId}")]
    public async Task<IActionResult> GetShoplistById(int shoplistId)
    {
        var shoplist = await _shoplistRepository.GetShopListByIdAsync(shoplistId);
        if (shoplist == null)
        {
            return NotFound("Shoplist not found.");
        }
        return Ok(shoplist);
    }

    [HttpPost]
    public async Task<IActionResult> CreateShoplist(Shoplist shoplist)
    {
        await _shoplistRepository.AddShopListsAsync(shoplist);
        return CreatedAtAction(nameof(GetShoplistById), new { shoplistId = shoplist.Id }, shoplist);
    }

    [HttpPut("{shoplistId}")]
    public async Task<IActionResult> UpdateShoplist(int shoplistId, Shoplist shoplist)
    {
        if (shoplistId != shoplist.Id)
        {
            return BadRequest("Shoplist ID mismatch.");
        }

        var existingShoplist = await _shoplistRepository.GetShopListByIdAsync(shoplistId);
        if (existingShoplist == null)
        {
            return NotFound("Shoplist not found.");
        }

        await _shoplistRepository.UpdateShopListsAsync(shoplist);
        return NoContent();
    }

    [HttpDelete("{shoplistId}")]
    public async Task<IActionResult> DeleteShoplist(int shoplistId)
    {
        var shoplist = await _shoplistRepository.GetShopListByIdAsync(shoplistId);
        if (shoplist == null)
        {
            return NotFound("Shoplist not found.");
        }

        await _productsInShoplistRepository.DeleteAllProductsFromShoplistAsync(shoplistId);
        await _shoplistRepository.DeleteShopListAsync(shoplistId);

        return NoContent();
    }
}
