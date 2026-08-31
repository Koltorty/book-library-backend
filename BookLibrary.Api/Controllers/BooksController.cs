using BookLibrary.Api.DTOs.BookDtos;
using BookLibrary.Api.DTOs.Common;
using BookLibrary.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController(BookService bookService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<PagedResult<BookListItemDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBooks(
        [FromQuery] BookFilter filter,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12)
    {
        var result = await bookService.GetBooks(filter, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<BookDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBook([FromRoute] int id)
    {
        var book = await bookService.GetBook(id);
        return book is not null ? Ok(book) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBook([FromBody] BookSaveDto dto)
    {
        var id = await bookService.AddBook(dto);
        return CreatedAtAction(nameof(GetBook), new { id }, new { id });
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateBook([FromRoute] int id, [FromBody] BookSaveDto dto)
    {
        var success = await bookService.UpdateBook(id, dto);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteBook([FromRoute] int id)
    {
        var success = await bookService.DeleteBook(id);
        return success ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/restore")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RestoreBook([FromRoute] int id)
    {
        var success = await bookService.RestoreBook(id);
        return success ? NoContent() : NotFound();
    }
}
