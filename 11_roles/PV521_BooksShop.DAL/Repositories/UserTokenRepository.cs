using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Repositories
{
    public class UserTokenRepository
        : GenericRepository<UserToken>
    {
        public UserTokenRepository(AppDbContext context)
            : base(context)
        {

        }
    }
}
