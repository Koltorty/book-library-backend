using BookLibrary.Api.DTOs.CategoryDtos;
using BookLibrary.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookLibrary.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController(CategoryService categoryService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<CategoryDto[]>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories([FromQuery] bool onlyActive)
    {
        var categories = await categoryService.GetCategories(onlyActive);
        return Ok(categories);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<CategoryDetailDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategory([FromRoute] int id)
    {
        var category = await categoryService.GetCategory(id);
        return category is not null ? Ok(category) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddCategory([FromBody] CategoryCreateDto dto)
    {
        var id = await categoryService.AddCategory(dto.Name);
        return CreatedAtAction(nameof(GetCategory), new {id}, new {id});
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryCreateDto dto)
    {
        var success = await categoryService.UpdateCategory(id, dto.Name);
        return success ? NoContent() : NotFound();
    }
}