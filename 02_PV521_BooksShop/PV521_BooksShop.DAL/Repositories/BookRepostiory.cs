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
    }
}
