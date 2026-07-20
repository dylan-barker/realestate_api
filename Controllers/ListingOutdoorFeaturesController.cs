using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Application.Interfaces;

namespace RealEstateApi.Controllers;

[ApiController]
[Authorize(Roles = "Admin,Agent")]
[Route("api/listings/{listingId}/outdoor-features")]
public class ListingOutdoorFeaturesController : ControllerBase
{
    private readonly IListingOutdoorFeatureService _outdoorFeatureService;

    public ListingOutdoorFeaturesController(IListingOutdoorFeatureService outdoorFeatureService)
    {
        _outdoorFeatureService = outdoorFeatureService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int listingId)
    {
        var result = await _outdoorFeatureService.GetByListingIdAsync(listingId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int listingId, [FromBody] AddOutdoorFeatureRequest request)
    {
        var result = await _outdoorFeatureService.AddAsync(listingId, request);
        return CreatedAtAction(nameof(GetAll), new { listingId }, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int listingId, int id)
    {
        await _outdoorFeatureService.DeleteAsync(listingId, id);
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> ReplaceAll(int listingId, [FromBody] ReplaceOutdoorFeaturesRequest request)
    {
        var result = await _outdoorFeatureService.ReplaceAllAsync(listingId, request);
        return Ok(result);
    }
}
