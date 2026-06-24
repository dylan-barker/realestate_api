using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Application.Interfaces;

namespace RealEstateApi.Controllers;

[ApiController]
[Route("api/listings")]
public class ListingsController : ControllerBase
{
    private readonly IListingService _listingService;

    public ListingsController(IListingService listingService)
    {
        _listingService = listingService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateListingRequest request)
    {
        var result = await _listingService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? status, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
    {
        var result = await _listingService.GetAllAsync(status, dateFrom, dateTo);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _listingService.GetByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateListingRequest request)
    {
        var result = await _listingService.UpdateAsync(id, request);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _listingService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/submit")]
    public async Task<IActionResult> Submit(int id)
    {
        var result = await _listingService.SubmitAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    // Address
    [HttpGet("{id}/address")]
    public async Task<IActionResult> GetAddress(int id)
    {
        var result = await _listingService.GetAddressAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id}/address")]
    public async Task<IActionResult> UpsertAddress(int id, [FromBody] UpsertAddressRequest request)
    {
        var result = await _listingService.UpsertAddressAsync(id, request);
        return Ok(result);
    }

    // Building Info
    [HttpGet("{id}/building-info")]
    public async Task<IActionResult> GetBuildingInfo(int id)
    {
        var result = await _listingService.GetBuildingInfoAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id}/building-info")]
    public async Task<IActionResult> UpsertBuildingInfo(int id, [FromBody] UpsertBuildingInfoRequest request)
    {
        var result = await _listingService.UpsertBuildingInfoAsync(id, request);
        return Ok(result);
    }

    // Valuation
    [HttpGet("{id}/valuation")]
    public async Task<IActionResult> GetValuation(int id)
    {
        var result = await _listingService.GetValuationAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id}/valuation")]
    public async Task<IActionResult> UpsertValuation(int id, [FromBody] UpsertValuationRequest request)
    {
        var result = await _listingService.UpsertValuationAsync(id, request);
        return Ok(result);
    }

    // Running Costs
    [HttpGet("{id}/running-costs")]
    public async Task<IActionResult> GetRunningCosts(int id)
    {
        var result = await _listingService.GetRunningCostsAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id}/running-costs")]
    public async Task<IActionResult> UpsertRunningCosts(int id, [FromBody] UpsertRunningCostsRequest request)
    {
        var result = await _listingService.UpsertRunningCostsAsync(id, request);
        return Ok(result);
    }
}
