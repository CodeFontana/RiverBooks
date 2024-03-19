using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RiverBooks.Books.Data.Entities;

namespace RiverBooks.Books.Data;
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
        yield return new Book(1, "The Fellowship of the Ring", "J.R.R Tolkien", 10.99m);
        yield return new Book(2, "The Two Towers", "J.R.R Tolkien", 11.99m);
        yield return new Book(3, "The Return of the King", "J.R.R Tolkien", 12.99m);
    }

}
