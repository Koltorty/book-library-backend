namespace BookLibrary.Api.DTOs.SeriesDtos;

public class SeriesListItemDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public IReadOnlyList<SeriesListItemDto> SubSeries { get; set; } = [];
}
