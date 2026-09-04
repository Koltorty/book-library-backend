namespace BookLibrary.Tests.Services;

public class AuthorServiceTests : ServiceTestBase
{
    [Fact]
    public async Task AddAuthor_WithSameName_ShouldReturnExistingId()
    {
        // Act
        var firstId = await _authorService.AddAuthor("Leo Tolstoy");
        var secondId = await _authorService.AddAuthor("Leo Tolstoy");

        // Assert
        Assert.Equal(firstId, secondId);

        var authors = await _authorService.GetAuthors(onlyActive: false);
        Assert.Single(authors);
    }
}
