using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Application.Interfaces;

namespace RealEstateApi.Controllers;

[ApiController]
[Route("api/listings/{listingId}/parking")]
public class ListingParkingController : ControllerBase
{
    private readonly IListingParkingService _parkingService;

    public ListingParkingController(IListingParkingService parkingService)
    {
        _parkingService = parkingService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int listingId)
    {
        var result = await _parkingService.GetParkingAsync(listingId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int listingId, [FromBody] AddParkingRequest request)
    {
        var result = await _parkingService.AddParkingAsync(listingId, request);
        return CreatedAtAction(nameof(GetAll), new { listingId }, result);
    }

    [HttpPut("{parkingId}")]
    public async Task<IActionResult> Update(int listingId, int parkingId, [FromBody] UpdateParkingRequest request)
    {
        var result = await _parkingService.UpdateParkingAsync(listingId, parkingId, request);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{parkingId}")]
    public async Task<IActionResult> Delete(int listingId, int parkingId)
    {
        await _parkingService.DeleteParkingAsync(listingId, parkingId);
        return NoContent();
    }
}
