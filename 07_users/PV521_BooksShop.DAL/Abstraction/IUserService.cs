using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Abstraction
{
    public interface IUserService
    {
        public Task CreateAsync(User user, string password, CancellationToken ct = default);
    }
}
