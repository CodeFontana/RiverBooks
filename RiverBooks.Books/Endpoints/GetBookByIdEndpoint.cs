using FastEndpoints;
using RiverBooks.Books.Interfaces;
using RiverBooks.Books.Models;
using RiverBooks.Books.Requests;

namespace RiverBooks.Books.Endpoints;

internal class GetBookByIdEndpoint(IBookService bookService) : Endpoint<GetBookByIdRequest, BookDto>
{
    private readonly IBookService _bookService = bookService;

    public override void Configure()
    {
        Get("/api/books/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetBookByIdRequest req, CancellationToken ct)
    {
        BookDto? book = await _bookService.GetBookByIdAsync(req.Id);

        if (book is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendAsync(book, cancellation: ct);
    }
}
