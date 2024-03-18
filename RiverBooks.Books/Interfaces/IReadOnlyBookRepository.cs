using RiverBooks.Books.Data.Entities;

namespace RiverBooks.Books.Interfaces;
internal interface IReadOnlyBookRepository
{
    Task<Book?> GetByIdAsync(int id);
    Task<List<Book>> ListAsync();
}
