using BookLibrary.Api.DTOs.SeriesDtos;
using BookLibrary.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SeriesController(SeriesService seriesService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<SeriesListItemDto[]>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSeries()
    {
        var series = await seriesService.GetSeries();
        return Ok(series);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<SeriesDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSeries([FromRoute] int id)
    {
        var series = await seriesService.GetSeries(id);
        return series is not null ? Ok(series) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSeries([FromBody] SeriesCreateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var id = await seriesService.AddSeries(dto);
        return Created($"/series/{id}", new { id });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSeries([FromRoute] int id, [FromBody] SeriesUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var success = await seriesService.UpdateSeries(id, dto);
        return success ? NoContent() : NotFound();
    }
}
