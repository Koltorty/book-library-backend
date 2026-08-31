namespace BookLibrary.Api.DTOs.SeriesDtos;

public class SeriesCreateDto
{
    public string Title { get; set; } = string.Empty;
    public int? ParentSeriesId { get; set; }
}
