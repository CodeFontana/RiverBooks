namespace RiverBooks.Books;

internal class BookService : IBookService
{
    public List<BookDto> ListBooks()
    {
        return [
            new BookDto(1, "The Hobbit", "J.R.R. Tolkien"),
            new BookDto(2, "The Fellowship of the Ring", "J.R.R. Tolkien"),
            new BookDto(3, "The Two Towers", "J.R.R. Tolkien"),
        ];
    }
}
