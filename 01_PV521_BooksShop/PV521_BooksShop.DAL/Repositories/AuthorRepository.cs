using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Repositories
{
    public class AuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public IQueryable<Author> Authors =>
            _context.Authors.AsNoTracking();

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _context.Authors
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> CreateAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<int> CreateRangeAsync(
            IEnumerable<Author> authors)
        {
            await _context.Authors.AddRangeAsync(authors);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Author author)
        {
            _context.Authors.Update(author);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(Author author)
        {
            _context.Authors.Remove(author);
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await GetByIdAsync(id);
            return author != null && await DeleteAsync(author);
        }
    }
}
