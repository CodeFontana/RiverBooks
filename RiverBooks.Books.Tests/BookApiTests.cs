using System.Net.Mime;
using System.Text;
using System.Text.Json;
using RiverBooks.Books.Models;

namespace RiverBooks.Books.Tests;

public class BookApiTests : IClassFixture<BooksApiApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public BookApiTests(BooksApiApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    }

    [Fact]
    public async Task CreateBookEndpoint_CreatesBooksSuccessfully()
    {
        // Arrange
        var booksToCreate = new[]
        {
            new { Title = "Moby Dick", Author = "Herman Melville", Price = 9.99m },
            new { Title = "Catcher in the Rye", Author = "J.D. Salinger", Price = 8.99m },
            new { Title = "To Kill a Mockingbird", Author = "Harper Lee", Price = 10.99m }
        };

        foreach (var book in booksToCreate)
        {
            // Act
            StringContent content = new(
                JsonSerializer.Serialize(book),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            HttpResponseMessage response = await _client.PostAsync("/api/v1/books", content);
            response.EnsureSuccessStatusCode();

            BookDto? returnedBook = JsonSerializer.Deserialize<BookDto>(
                await response.Content.ReadAsStringAsync(),
                _jsonOptions);

            // Assert
            Assert.NotNull(returnedBook);
            Assert.Equal(book.Title, returnedBook.Title);
            Assert.Equal(book.Author, returnedBook.Author);
            Assert.Equal(book.Price, returnedBook.Price);
            Assert.True(returnedBook.Id.HasValue && returnedBook.Id.Value > 0);
        }
    }

    [Fact]
    public async Task ListBooksEndpoint_IncludesCreatedBooks()
    {
        // Arrange
        var booksToCreate = new[]
        {
            new { Title = "Moby Dick", Author = "Herman Melville", Price = 9.99m },
            new { Title = "Catcher in the Rye", Author = "J.D. Salinger", Price = 8.99m },
            new { Title = "To Kill a Mockingbird", Author = "Harper Lee", Price = 10.99m }
        };

        // Act
        foreach (var book in booksToCreate)
        {
            StringContent content = new(
                JsonSerializer.Serialize(book),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
            HttpResponseMessage response = await _client.PostAsync("/api/v1/books", content);
            response.EnsureSuccessStatusCode();
        }

        HttpResponseMessage listResponse = await _client.GetAsync("/api/v1/books");
        listResponse.EnsureSuccessStatusCode();

        List<BookDto>? booksList = JsonSerializer.Deserialize<List<BookDto>>(
            await listResponse.Content.ReadAsStringAsync(),
            _jsonOptions);

        // Assert
        Assert.NotNull(booksList);
        Assert.True(booksList.Count >= booksToCreate.Length);

        foreach (var book in booksToCreate)
        {
            Assert.Contains(booksList, b => b.Title == book.Title && b.Author == book.Author && b.Price == book.Price);
        }
    }

    [Fact]
    public async Task GetBookByIdEndpoint_RetrievesCorrectBook()
    {
        // Arrange
        var newBook = new { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Price = 7.99m };
        StringContent createContent = new(
            JsonSerializer.Serialize(newBook),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act
        HttpResponseMessage createResponse = await _client.PostAsync("/api/v1/books", createContent);
        createResponse.EnsureSuccessStatusCode();

        BookDto? createdBook = JsonSerializer.Deserialize<BookDto>(
            await createResponse.Content.ReadAsStringAsync(),
            _jsonOptions);

        // Assert -- Book created
        Assert.NotNull(createdBook);
        Assert.True(createdBook.Id.HasValue && createdBook.Id.Value > 0);

        // Act (again)
        HttpResponseMessage getResponse = await _client.GetAsync($"/api/v1/books/{createdBook.Id.Value}");
        getResponse.EnsureSuccessStatusCode();

        BookDto? retrievedBook = JsonSerializer.Deserialize<BookDto>(
            await getResponse.Content.ReadAsStringAsync(),
            _jsonOptions);

        // Assert -- Book retrieved
        Assert.NotNull(retrievedBook);
        Assert.Equal(createdBook.Id, retrievedBook.Id);
        Assert.Equal(newBook.Title, retrievedBook.Title);
        Assert.Equal(newBook.Author, retrievedBook.Author);
        Assert.Equal(newBook.Price, retrievedBook.Price);
    }

    [Fact]
    public async Task UpdateBookPrice_EndpointUpdatesBookPriceSuccessfully()
    {
        // Arrange
        var magicEyeBook = new { Title = "Magic Eye: A New Way of Looking at the World", Author = "N.E. Thing Enterprises", Price = 15.99m };
        StringContent createContent = new(
            JsonSerializer.Serialize(magicEyeBook),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act -- Create the book
        HttpResponseMessage createResponse = await _client.PostAsync("/api/v1/books", createContent);
        createResponse.EnsureSuccessStatusCode();

        BookDto? createdBook = JsonSerializer.Deserialize<BookDto>(
            await createResponse.Content.ReadAsStringAsync(),
            _jsonOptions);

        // Assert
        Assert.NotNull(createdBook);
        Assert.True(createdBook.Id.HasValue && createdBook.Id.Value > 0);

        // Arrange -- Update the price
        decimal newPrice = 19.99m;

        // Act -- Update the price
        HttpResponseMessage updateResponse = await _client.PostAsync($"/api/v1/books/{createdBook.Id.Value}/pricehistory?price={newPrice}", null);
        updateResponse.EnsureSuccessStatusCode();

        HttpResponseMessage getResponse = await _client.GetAsync($"/api/v1/books/{createdBook.Id.Value}");
        getResponse.EnsureSuccessStatusCode();

        BookDto? updatedBook = JsonSerializer.Deserialize<BookDto>(
            await getResponse.Content.ReadAsStringAsync(),
            _jsonOptions);

        // Assert -- Price updated
        Assert.NotNull(updatedBook);
        Assert.Equal(newPrice, updatedBook.Price);
    }

    [Fact]
    public async Task DeleteBookEndpoint_RemovesBookSuccessfully()
    {
        // Arrange
        var bookToDelete = new { Title = "Magic Eye: Beyond 3D", Author = "N.E. Thing Enterprises", Price = 12.99m };
        StringContent createContent = new(
            JsonSerializer.Serialize(bookToDelete),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

        // Act -- Create the book
        HttpResponseMessage createResponse = await _client.PostAsync("/api/v1/books", createContent);
        createResponse.EnsureSuccessStatusCode();

        BookDto? createdBook = JsonSerializer.Deserialize<BookDto>(
            await createResponse.Content.ReadAsStringAsync(),
            _jsonOptions);

        // Assert -- Book created
        Assert.NotNull(createdBook);
        Assert.True(createdBook.Id.HasValue && createdBook.Id.Value > 0);

        // Act -- Delete the book
        HttpResponseMessage deleteResponse = await _client.DeleteAsync($"/api/v1/books/{createdBook.Id.Value}");
        deleteResponse.EnsureSuccessStatusCode();

        HttpResponseMessage getResponse = await _client.GetAsync($"/api/v1/books/{createdBook.Id.Value}");

        // Assert -- Book deleted
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }

}