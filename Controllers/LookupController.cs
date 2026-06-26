using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Application.Interfaces;

namespace RealEstateApi.Controllers;

[ApiController]
[Authorize(Roles = "Admin,Agent")]
[Route("api")]
public class LookupController : ControllerBase
{
    private readonly ILookupService _lookupService;

    public LookupController(ILookupService lookupService)
    {
        _lookupService = lookupService;
    }

    [HttpGet("property-types")]
    public async Task<IActionResult> GetPropertyTypes()
    {
        var result = await _lookupService.GetPropertyTypesAsync();
        return Ok(result);
    }

    [HttpGet("room-types")]
    public async Task<IActionResult> GetRoomTypes()
    {
        var result = await _lookupService.GetRoomTypesAsync();
        return Ok(result);
    }

    [HttpGet("features")]
    public async Task<IActionResult> GetFeatures()
    {
        var result = await _lookupService.GetFeaturesAsync();
        return Ok(result);
    }

    [HttpGet("condition-categories")]
    public async Task<IActionResult> GetConditionCategories()
    {
        var result = await _lookupService.GetConditionCategoriesAsync();
        return Ok(result);
    }

    [HttpGet("parking-types")]
    public async Task<IActionResult> GetParkingTypes()
    {
        var result = await _lookupService.GetParkingTypesAsync();
        return Ok(result);
    }

    [HttpGet("facing")]
    public async Task<IActionResult> GetFacing()
    {
        var result = await _lookupService.GetFacingAsync();
        return Ok(result);
    }

    [HttpGet("zoning")]
    public async Task<IActionResult> GetZoning()
    {
        var result = await _lookupService.GetZoningAsync();
        return Ok(result);
    }
}
