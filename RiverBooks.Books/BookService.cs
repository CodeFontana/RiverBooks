namespace RiverBooks.Books;

internal class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<int> CreateBookAsync(BookDto newBook)
    {
        Book book = new(newBook.Id, newBook.Title, newBook.Author, newBook.Price);
        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();
        return book.Id;
    }

    public async Task DeleteBookAsync(int id)
    {
        Book? bookToDelete = await _bookRepository.GetByIdAsync(id);

        if (bookToDelete is not null)
        {
            await _bookRepository.DeleteAsync(bookToDelete);
            await _bookRepository.SaveChangesAsync();
        }
    }

    public async Task<BookDto?> GetBookByIdAsync(int id)
    {
        Book? book = await _bookRepository.GetByIdAsync(id);

        if (book is not null)
        {
            return new BookDto(book.Id, book.Title, book.Author, book.Price);
        }

        return null;
    }

    public async Task<List<BookDto>> ListBooksAsync()
    {
        List<BookDto> books = (await _bookRepository.ListAsync())
            .Select(book => new BookDto(book.Id, book.Title, book.Author, book.Price))
            .ToList();

        return books;
    }

    public async Task UpdateBookPriceAsync(int id, decimal newPrice)
    {
        Book? book = await _bookRepository.GetByIdAsync(id);

        if (book is not null)
        {
            book.UpdatePrice(newPrice);
            await _bookRepository.SaveChangesAsync();
        }

        throw new InvalidOperationException("Book not found");
    }
}
