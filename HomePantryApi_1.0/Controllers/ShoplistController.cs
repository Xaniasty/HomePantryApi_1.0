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

    //[HttpPut("{shoplistId}")]
    //public async Task<IActionResult> UpdateShoplist(int shoplistId, Shoplist shoplist)
    //{
    //    if (shoplistId != shoplist.Id)
    //    {
    //        return BadRequest("Shoplist ID mismatch.");
    //    }

    //    var existingShoplist = await _shoplistRepository.GetShopListByIdAsync(shoplistId);
    //    if (existingShoplist == null)
    //    {
    //        return NotFound("Shoplist not found.");
    //    }

    //    await _shoplistRepository.UpdateShopListsAsync(shoplist);
    //    return NoContent();
    //}

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

        existingShoplist.DataAktualizacji = shoplist.DataAktualizacji;
        existingShoplist.ShoplistName = shoplist.ShoplistName;
        existingShoplist.Opis = shoplist.Opis;

        await _shoplistRepository.UpdateShopListsAsync(existingShoplist);
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


    [HttpPost("CreateShoplistFromGranary/user/{userId}/{granaryId}")]
    public async Task<IActionResult> CreateShoplistFromGranary(int granaryId, int userId)
    {
        try
        {
            var newShoplist = await _shoplistRepository.CreateShoplistFromGranaryAsync(granaryId, userId);
            return CreatedAtAction(nameof(GetShoplistById), new { shoplistId = newShoplist.Id }, newShoplist);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Wystąpił błąd podczas tworzenia listy zakupów. {ex.Message}");
        }
    }

    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeleteAllGranariesForUser(int userId)
    {
        try
        {
            var userShoplists = await _shoplistRepository.GetAllShopListsForUserAsync(userId);
            if (!userShoplists.Any())
            {
                return NotFound("No shoplists found for this user.");
            }

            await _shoplistRepository.DeleteShopListsForUserAsync(userId);

            return Ok("All shoplists for the user have been deleted.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting shoplists for the user. {ex.Message}");
        }
    }



}
