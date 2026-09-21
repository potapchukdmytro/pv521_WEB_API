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
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

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

                e.Property(b => b.Image)
                .HasMaxLength(100);

                e.Property(b => b.Language)
                .HasMaxLength(50);

                e.Property(b => b.Isbn)
                .HasMaxLength(25);

                e.Property(b => b.Publisher)
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

            // Role
            builder.Entity<Role>(e =>
            {
                e.HasKey(r => r.Id);

                e.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(50);
            });

            // User
            builder.Entity<User>(e =>
            {
                e.HasKey(u => u.Id);

                e.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(64);

                e.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

                e.Property(u => u.PasswordHash)
                .HasMaxLength(128);

                e.Property(u => u.FirstName)
                .HasMaxLength(100);

                e.Property(u => u.LastName)
                .HasMaxLength(100);

                e.Property(u => u.Image)
               .HasMaxLength(100);

                e.Property(u => u.PhoneNumber)
               .HasMaxLength(15);
            });

            // Relationships
            builder.Entity<Book>()
                .HasOne(b => b.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
