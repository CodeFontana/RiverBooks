using FastEndpoints;

namespace RiverBooks.Books;

internal class CreateBookEndpoint(IBookService bookService) : Endpoint<CreateBookRequest, Book>
{
    public override void Configure()
    {
        Post("/api/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateBookRequest req, CancellationToken ct)
    {
        Book newBook = new(req.Title, req.Author, req.Price);
        await bookService.CreateBookAsync(newBook);
        await SendCreatedAtAsync<GetBookByIdEndpoint>(new { newBook.Id }, newBook, cancellation: ct);
    }
}
