using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Application.Interfaces;

namespace RealEstateApi.Controllers;

[ApiController]
[Route("api/listings/{listingId}/rooms")]
public class ListingRoomsController : ControllerBase
{
    private readonly IListingRoomService _roomService;

    public ListingRoomsController(IListingRoomService roomService)
    {
        _roomService = roomService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int listingId)
    {
        var result = await _roomService.GetRoomsAsync(listingId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int listingId, [FromBody] CreateRoomRequest request)
    {
        var result = await _roomService.CreateRoomAsync(listingId, request);
        return CreatedAtAction(nameof(GetAll), new { listingId }, result);
    }

    [HttpPut("{roomId}")]
    public async Task<IActionResult> Update(int listingId, int roomId, [FromBody] UpdateRoomRequest request)
    {
        var result = await _roomService.UpdateRoomAsync(listingId, roomId, request);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{roomId}")]
    public async Task<IActionResult> Delete(int listingId, int roomId)
    {
        await _roomService.DeleteRoomAsync(listingId, roomId);
        return NoContent();
    }

    // Condition
    [HttpPut("{roomId}/condition")]
    public async Task<IActionResult> UpsertCondition(int listingId, int roomId, [FromBody] UpsertRoomConditionRequest request)
    {
        var result = await _roomService.UpsertConditionAsync(listingId, roomId, request);
        return Ok(result);
    }

    // Features (junction)
    [HttpPost("{roomId}/features")]
    public async Task<IActionResult> LinkFeature(int listingId, int roomId, [FromBody] LinkFeatureRequest request)
    {
        var result = await _roomService.LinkFeatureAsync(listingId, roomId, request.FeatureId);
        return Ok(result);
    }

    [HttpDelete("{roomId}/features/{featureId}")]
    public async Task<IActionResult> UnlinkFeature(int listingId, int roomId, int featureId)
    {
        var result = await _roomService.UnlinkFeatureAsync(listingId, roomId, featureId);
        return Ok(result);
    }

    // Custom Features
    [HttpPost("{roomId}/custom-features")]
    public async Task<IActionResult> AddCustomFeature(int listingId, int roomId, [FromBody] AddCustomFeatureRequest request)
    {
        var result = await _roomService.AddCustomFeatureAsync(listingId, roomId, request);
        return CreatedAtAction(nameof(GetAll), new { listingId }, result);
    }

    [HttpDelete("{roomId}/custom-features/{customFeatureId}")]
    public async Task<IActionResult> DeleteCustomFeature(int listingId, int roomId, int customFeatureId)
    {
        await _roomService.DeleteCustomFeatureAsync(listingId, roomId, customFeatureId);
        return NoContent();
    }
}
