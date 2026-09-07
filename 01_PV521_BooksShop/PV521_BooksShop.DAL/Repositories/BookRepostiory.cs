using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Repositories
{
    public class BookRepostiory
    {
        private readonly AppDbContext _context;

        public BookRepostiory(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Book> Books => _context.Books.AsNoTracking();

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<bool> CreateAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            int res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<int> CreateRangeAsync(IEnumerable<Book> books)
        {
            var tasks = books.Select(b => _context.AddAsync(b).AsTask());
            await Task.WhenAll(tasks);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            int res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> DeleteAsync(Book book)
        {
            _context.Books.Remove(book);
            int res = await _context.SaveChangesAsync();
            return res > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await GetByIdAsync(id);
            return book != null && await DeleteAsync(book);
        }
    }
}
