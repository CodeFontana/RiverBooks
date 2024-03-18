using RiverBooks.Books.Models;

namespace RiverBooks.Books.Responses;

public class ListBooksResponse
{
    public List<BookDto> Books { get; set; } = new();
}
