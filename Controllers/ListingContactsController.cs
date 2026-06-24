using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Application.DTOs;
using RealEstateApi.Application.Interfaces;

namespace RealEstateApi.Controllers;

[ApiController]
[Route("api/listings/{listingId}/contacts")]
public class ListingContactsController : ControllerBase
{
    private readonly IListingContactService _contactService;

    public ListingContactsController(IListingContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(int listingId)
    {
        var result = await _contactService.GetContactsAsync(listingId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(int listingId, [FromBody] AddContactRequest request)
    {
        var result = await _contactService.AddContactAsync(listingId, request);
        return CreatedAtAction(nameof(GetAll), new { listingId }, result);
    }

    [HttpPut("{contactId}")]
    public async Task<IActionResult> Update(int listingId, int contactId, [FromBody] UpdateContactRequest request)
    {
        var result = await _contactService.UpdateContactAsync(listingId, contactId, request);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpDelete("{contactId}")]
    public async Task<IActionResult> Delete(int listingId, int contactId)
    {
        await _contactService.DeleteContactAsync(listingId, contactId);
        return NoContent();
    }
}
