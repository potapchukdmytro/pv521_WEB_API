using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Book
            builder.Entity<Book>(e =>
            {
                e.HasKey(b => b.Id);

                e.Property(b => b.Title)
                .HasMaxLength(255)
                .IsRequired();

                e.Property(b => b.Description)
                .HasColumnType("text");

                e.Property(b => b.Image)
                .HasMaxLength(100);
            });

            // Author
            builder.Entity<Author>(e =>
            {
                e.HasKey(a => a.Id);

                e.Property(a => a.Name)
                .HasMaxLength(255)
                .IsRequired();

                e.Property(a => a.Biography)
                .HasColumnType("text");

                e.Property(a => a.Image)
                .HasMaxLength(100);

                e.Property(a => a.Country)
                .HasMaxLength(255);
            });

            // Relationships
            builder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
