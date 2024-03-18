
using Microsoft.EntityFrameworkCore;

namespace RiverBooks.Books;

internal class EfBookRepository : IBookRepository
{
    private readonly BookDbContext _db;

    public EfBookRepository(BookDbContext db)
    {
        _db = db;
    }

    public Task AddAsync(Book book)
    {
        _db.Add(book);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Book book)
    {
        _db.Remove(book);
        return Task.CompletedTask;
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _db.Books.FindAsync(id);
    }

    public async Task<List<Book>> ListAsync()
    {
        return await _db.Books.ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
