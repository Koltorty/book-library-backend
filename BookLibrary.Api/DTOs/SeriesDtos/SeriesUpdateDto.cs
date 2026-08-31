namespace BookLibrary.Api.DTOs.SeriesDtos;

public class SeriesUpdateDto
{
    public string Title { get; set; } = string.Empty;
    public int? ParentSeriesId { get; set; }
}
