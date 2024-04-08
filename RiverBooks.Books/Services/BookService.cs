using RiverBooks.Books.Data.Entities;
using RiverBooks.Books.Interfaces;
using RiverBooks.Books.Models;

namespace RiverBooks.Books.Services;

internal class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookResponse> CreateBookAsync(BookRequest newBook)
    {
        Book book = new(newBook.Title, newBook.Author, newBook.Price);
        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();
        return new BookResponse(book.Id, book.Title, book.Author, book.Price);
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

    public async Task<BookResponse?> GetBookByIdAsync(int id)
    {
        Book? book = await _bookRepository.GetByIdAsync(id);

        if (book is not null)
        {
            return new BookResponse(book.Id, book.Title, book.Author, book.Price);
        }

        return null;
    }

    public async Task<List<BookResponse>> ListBooksAsync()
    {
        List<BookResponse> books = (await _bookRepository.ListAsync())
            .Select(book => new BookResponse(book.Id, book.Title, book.Author, book.Price))
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
            return;
        }

        throw new InvalidOperationException("Book not found");
    }
}
