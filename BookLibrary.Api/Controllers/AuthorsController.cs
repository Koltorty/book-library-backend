using BookLibrary.Api.DTOs.AuthorDtos;
using BookLibrary.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorsController(AuthorService authorService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<AuthorListItemDto[]>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAuthors()
    {
        var authors = await authorService.GetAuthors();
        return Ok(authors);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<AuthorDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAuthor([FromRoute] int id)
    {
        var author = await authorService.GetAuthor(id);
        return author is not null ? Ok(author) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAuthor([FromBody] AuthorCreateDto dto)
    {
        var id = await authorService.AddAuthor(dto.Name);
        return CreatedAtAction(nameof(GetAuthor), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAuthor([FromRoute] int id, [FromBody] AuthorCreateDto dto)
    {
        var success = await authorService.UpdateAuthor(id, dto.Name);
        return success ? NoContent() : NotFound();
    }
}
