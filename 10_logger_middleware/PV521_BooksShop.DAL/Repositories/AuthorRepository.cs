using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Repositories
{
    public class AuthorRepository : GenericRepository<Author>
    {
        public AuthorRepository(AppDbContext context)
            : base(context)
        {

        }

        public IQueryable<Author> Authors => GetAll();
    }
}
