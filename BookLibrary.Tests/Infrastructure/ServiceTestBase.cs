using BookLibrary.Api.Services;
using BookLibrary.Tests.Infrastructure;

namespace BookLibrary.Tests.Services;

public abstract class ServiceTestBase
{
    protected readonly DbFactory _dbFactory = new();
    protected readonly BookService _bookService;
    protected readonly AuthorService _authorService;
    protected readonly CategoryService _categoryService;
    protected readonly PublisherService _publisherService;
    protected readonly SeriesService _seriesService;

    protected ServiceTestBase()
    {
        _bookService = new BookService(_dbFactory);
        _authorService = new AuthorService(_dbFactory);
        _categoryService = new CategoryService(_dbFactory);
        _publisherService = new PublisherService(_dbFactory);
        _seriesService = new SeriesService(_dbFactory);
    }
}
