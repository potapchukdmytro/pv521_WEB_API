using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Repositories
{
    public class BookRepostiory : GenericRepository<Book>
    {
        private readonly AppDbContext _context;

        public BookRepostiory(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<Book> Books => GetAll();

        public async Task<Book?> GetByIdAsync(int id, bool include = false)
        {
            var entity =  await base.GetByIdAsync(id);

            if (include && entity != null)
            {
                await _context.Entry(entity).Reference(e => e.Author).LoadAsync();
            }

            return entity;
        }
    }
}
