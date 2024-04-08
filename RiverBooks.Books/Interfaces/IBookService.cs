using RiverBooks.Books.Models;

namespace RiverBooks.Books.Interfaces;

internal interface IBookService
{
    Task<List<BookResponse>> ListBooksAsync();
    Task<BookResponse?> GetBookByIdAsync(int id);
    Task<BookResponse> CreateBookAsync(BookRequest newBook);
    Task DeleteBookAsync(int id);
    Task UpdateBookPriceAsync(int id, decimal newPrice);
}
