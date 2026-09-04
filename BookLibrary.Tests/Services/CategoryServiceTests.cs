namespace BookLibrary.Tests.Services;

public class CategoryServiceTests : ServiceTestBase
{
    [Fact]
    public async Task AddCategory_WithSameName_ShouldReturnExistingId()
    {
        // Act
        var firstId = await _categoryService.AddCategory("Fiction");
        var secondId = await _categoryService.AddCategory("Fiction");

        // Assert
        Assert.Equal(firstId, secondId);

        var categories = await _categoryService.GetCategories(onlyActive: false);
        Assert.Single(categories);
    }
}
