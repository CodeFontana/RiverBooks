using FastEndpoints;

namespace RiverBooks.Books;

internal class CreateBookEndpoint(IBookService bookService) : Endpoint<CreateBookRequest, BookDto>
{
    public override void Configure()
    {
        Post("/api/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBookRequest req, CancellationToken ct)
    {
        BookDto newBook = new(0, req.Title, req.Author, req.Price);
        int id = await bookService.CreateBookAsync(newBook);
        newBook = newBook with { Id = id };
        await SendCreatedAtAsync<GetBookByIdEndpoint>(new { newBook.Id }, newBook, cancellation: ct);
    }
}
