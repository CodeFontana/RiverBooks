using FastEndpoints;
using RiverBooks.Books.Interfaces;
using RiverBooks.Books.Models;
using RiverBooks.Books.Requests;

namespace RiverBooks.Books.Endpoints;

internal class UpdateBookPriceEndpoint(IBookService bookService) : Endpoint<UpdateBookPriceRequest, BookDto>
{
    private readonly IBookService _bookService = bookService;

    public override void Configure()
    {
        Post("/api/books/{id}/pricehistory");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateBookPriceRequest req, CancellationToken ct)
    {
        await _bookService.UpdateBookPriceAsync(req.Id, req.Price);
        BookDto? updatedBook = await _bookService.GetBookByIdAsync(req.Id);

        if (updatedBook != null)
        {
            await SendAsync(updatedBook, cancellation: ct);
            return;
        }

        await SendNotFoundAsync(ct);
    }
}
