﻿namespace RiverBooks.Books;

internal interface IBookService
{
    Task<List<BookDto>> ListBooksAsync();
    Task<BookDto?> GetBookByIdAsync(int id);
    Task<int> CreateBookAsync(BookDto newBook);
    Task DeleteBookAsync(int id);
    Task UpdateBookPriceAsync(int id, decimal newPrice);
}
