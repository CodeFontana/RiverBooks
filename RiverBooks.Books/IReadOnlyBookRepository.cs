namespace RiverBooks.Books;
internal interface IReadOnlyBookRepository
{
    Task<Book?> GetByIdAsync(int id);
    Task<List<Book>> ListAsync();
}
