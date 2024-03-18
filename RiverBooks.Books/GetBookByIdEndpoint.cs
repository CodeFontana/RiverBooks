using FastEndpoints;

namespace RiverBooks.Books;

internal class GetBookByIdEndpoint(IBookService bookService) : Endpoint<GetBookByIdRequest, Book>
{
    private readonly IBookService _bookService = bookService;

    public override void Configure()
    {
        Get("/api/books/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetBookByIdRequest req, CancellationToken ct)
    {
        Book? book = await _bookService.GetBookByIdAsync(req.Id);

        if (book is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(book, cancellation: ct);
    }
}
