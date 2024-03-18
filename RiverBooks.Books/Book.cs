namespace RiverBooks.Books;

public class Book
{
    public int Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public decimal Price { get; private set; }

    internal Book(int id, string title, string author, decimal price)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(title));
        ArgumentException.ThrowIfNullOrWhiteSpace(nameof(author));

        if (price < 0)
        {
            throw new ArgumentException("Price cannot be negative", nameof(price));
        }

        Id = id;
        Title = title;
        Author = author;
        Price = price;
    }

    internal void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
        {
            throw new ArgumentException("Price cannot be negative", nameof(newPrice));
        }

        Price = newPrice;
    }
}
