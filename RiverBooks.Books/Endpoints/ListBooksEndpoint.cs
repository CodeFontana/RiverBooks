using FastEndpoints;
using RiverBooks.Books.Interfaces;
using RiverBooks.Books.Models;
using RiverBooks.Books.Responses;

namespace RiverBooks.Books.Endpoints;

internal class ListBooksEndpoint(IBookService bookService) : EndpointWithoutRequest<ListBooksResponse>
{
    private readonly IBookService _bookService = bookService;

    public override void Configure()
    {
        Get("/api/books");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken cancellationToken = default)
    {
        List<BookDto> books = await _bookService.ListBooksAsync();

        await SendAsync(new ListBooksResponse()
        {
            Books = books
        }, cancellation: cancellationToken);
    }
}