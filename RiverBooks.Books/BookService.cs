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

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        Book? book = await _bookRepository.GetByIdAsync(id);
        return book;
    }

    public async Task<List<Book>> ListBooksAsync()
    {
        List<Book> books = await _bookRepository.ListAsync();
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
