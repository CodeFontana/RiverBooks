using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RiverBooks.Books.Interfaces;
using RiverBooks.Books.Models;

namespace RiverBooks.Books.Endpoints;

public static class BookApi
{
    public static void AddBookApiEndpoints(this WebApplication app)
    {
        app.MapGet("/api/v1/books", ReadAllAsync);
        app.MapGet("/api/v1/books/{id:int}", ReadAsync);
        app.MapPost("/api/v1/books", CreateAsync);
        app.MapPost("/api/v1/books/{id:int}/pricehistory", UpdateAsync);
        app.MapDelete("/api/v1/books/{id:int}", DeleteAsync);
    }

    private static async Task<IResult> ReadAllAsync(IBookService bookService)
    {
        try
        {
            List<BookDto> result = await bookService.ListBooksAsync();
            return Results.Ok(result);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message, statusCode: 500);
        }
    }

    private static async Task<IResult> ReadAsync(IBookService bookService, int id)
    {
        try
        {
            BookDto? result = await bookService.GetBookByIdAsync(id);

            if (result is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(result);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message, statusCode: 500);
        }
    }

    private static async Task<IResult> CreateAsync(IBookService bookService, BookDto newBook)
    {
        try
        {
            int id = await bookService.CreateBookAsync(newBook);
            newBook = newBook with { Id = id };
            return Results.Ok(newBook);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message, statusCode: 500);
        }
    }

    private static async Task<IResult> UpdateAsync(IBookService bookService, int id, decimal price)
    {
        try
        {
            await bookService.UpdateBookPriceAsync(id, price);
            BookDto? updatedBook = await bookService.GetBookByIdAsync(id);
            return Results.Ok(updatedBook);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message, statusCode: 500);
        }
    }

    private static async Task<IResult> DeleteAsync(IBookService bookService, int id)
    {
        try
        {
            await bookService.DeleteBookAsync(id);
            return Results.NoContent();
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message, statusCode: 500);
        }
    }
}
