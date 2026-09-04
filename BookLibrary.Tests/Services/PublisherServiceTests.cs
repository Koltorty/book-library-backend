namespace BookLibrary.Tests.Services;

public class PublisherServiceTests : ServiceTestBase
{
    [Fact]
    public async Task AddPublisher_WithSameName_ShouldReturnExistingId()
    {
        // Act
        var firstId = await _publisherService.AddPublisher("AST");
        var secondId = await _publisherService.AddPublisher("AST");

        // Assert
        Assert.Equal(firstId, secondId);

        var publishers = await _publisherService.GetPublishers(onlyActive: false);
        Assert.Single(publishers);
    }
}
