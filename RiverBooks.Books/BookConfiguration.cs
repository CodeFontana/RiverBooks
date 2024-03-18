using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RiverBooks.Books;
internal class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.Property(p => p.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Author)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasData(GetSampleData());
    }

    private static IEnumerable<Book> GetSampleData()
    {
        return [
            new Book(1, "The Fellowship of the Ring", "J.R.R Tolkien", 10.99m),
            new Book(2, "The Two Towers", "J.R.R Tolkien", 11.99m),
            new Book(3, "The Return of the King", "J.R.R Tolkien", 12.99m)
        ];
    }
}
