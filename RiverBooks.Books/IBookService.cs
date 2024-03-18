namespace RiverBooks.Books;

internal interface IBookService
{
    Task<List<Book>> ListBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    Task<int> CreateBookAsync(Book newBook);
    Task DeleteBookAsync(int id);
    Task UpdateBookPriceAsync(int id, decimal newPrice);
}
