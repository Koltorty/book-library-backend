using BookLibrary.Api.DTOs.PublisherDtos;
using BookLibrary.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PublishersController(PublisherService publisherService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<PublisherListItemDto[]>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPublishers()
    {
        var publishers = await publisherService.GetPublishers();
        return Ok(publishers);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<PublisherDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPublisher([FromRoute] int id)
    {
        var publisher = await publisherService.GetPublisher(id);
        return publisher is not null ? Ok(publisher) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddPublisher([FromBody] PublisherCreateDto dto)
    {
        var id = await publisherService.AddPublisher(dto.Name);
        return CreatedAtAction(nameof(GetPublisher), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePublisher([FromRoute] int id, [FromBody] PublisherCreateDto dto)
    {
        var success = await publisherService.UpdatePublisher(id, dto.Name);
        return success ? NoContent() : NotFound();
    }
}
