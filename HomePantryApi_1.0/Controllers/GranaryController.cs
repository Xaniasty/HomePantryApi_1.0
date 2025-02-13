﻿using HomePantryApi_1._0.Models;
using HomePantryApi_1._0.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace HomePantryApi_1._0.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GranaryController : ControllerBase
{
    private readonly IGranaryRepository _granaryRepository;
    private readonly IProductsInGranaryRepository _productsInGranaryRepository;

    public GranaryController(IGranaryRepository granaryRepository, IProductsInGranaryRepository productsInGranaryRepository)
    {
        _granaryRepository = granaryRepository;
        _productsInGranaryRepository = productsInGranaryRepository;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAllGranariesForUser(int userId)
    {
        var granaries = await _granaryRepository.GetAllGranariesForUserAsync(userId);
        return Ok(granaries);
    }

    [HttpGet("granary/{granaryId}")]
    public async Task<IActionResult> GetGranaryById(int granaryId)
    {
        var granary = await _granaryRepository.GetGranaryByIdAsync(granaryId);
        if (granary == null)
        {
            return NotFound("Granary not found.");
        }
        return Ok(granary);
    }

    [HttpPost]
    public async Task<IActionResult> CreateGranary(Granary granary)
    {
        await _granaryRepository.AddGranaryAsync(granary);
        return CreatedAtAction(nameof(GetGranaryById), new { granaryId = granary.Id }, granary);
    }

    //[HttpPut("{granaryId}")]
    //public async Task<IActionResult> UpdateGranary(int granaryId, Granary granary)
    //{
    //    if (granaryId != granary.Id)
    //    {
    //        return BadRequest("Granary ID mismatch.");
    //    }

    //    var existingGranary = await _granaryRepository.GetGranaryByIdAsync(granaryId);
    //    if (existingGranary == null)
    //    {
    //        return NotFound("Granary not found.");
    //    }

    //    await _granaryRepository.UpdateGranaryAsync(granary);
    //    return NoContent();
    //}


    [HttpPut("{granaryId}")]
    public async Task<IActionResult> UpdateGranary(int granaryId, Granary granary)
    {
        if (granaryId != granary.Id)
        {
            return BadRequest("Granary ID mismatch.");
        }

        var existingGranary = await _granaryRepository.GetGranaryByIdAsync(granaryId);
        if (existingGranary == null)
        {
            return NotFound("Granary not found.");
        }

        existingGranary.DataAktualizacji = granary.DataAktualizacji;
        existingGranary.GranaryName = granary.GranaryName;
        existingGranary.Opis = granary.Opis;

        await _granaryRepository.UpdateGranaryAsync(existingGranary);
        return NoContent();
    }

    [HttpDelete("{granaryId}")]
    public async Task<IActionResult> DeleteGranary(int granaryId)
    {
        var granary = await _granaryRepository.GetGranaryByIdAsync(granaryId);
        if (granary == null)
        {
            return NotFound("Granary not found.");
        }

        await _productsInGranaryRepository.DeleteAllProductsFromGranaryAsync(granaryId);
        await _granaryRepository.DeleteGranaryAsync(granaryId);

        return NoContent();
    }

    [HttpPost("CreateFromShoplist/user/{userId}/{shoplistId}")]
    public async Task<IActionResult> CreateGranaryFromShoplist(int shoplistId, int userId)
    {
        try
        {
            var newGranary = await _granaryRepository.CreateGranaryFromShoplistAsync(shoplistId, userId);
            return CreatedAtAction(nameof(GetGranaryById), new { granaryId = newGranary.Id }, newGranary);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error during creating process {ex.Message}");
        }
    }


    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeleteAllGranariesForUser(int userId)
    {
        try
        {
            var userGranaries = await _granaryRepository.GetAllGranariesForUserAsync(userId);
            if (!userGranaries.Any())
            {
                return NotFound("No granaries found for this user.");
            }

            await _granaryRepository.DeleteAllGranariesForUserAsync(userId);

            return Ok("All granaries for the user have been deleted.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while deleting granaries for the user. {ex.Message}");
        }
    }


}
