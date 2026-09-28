using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IQueryable<User> Users => GetAll();

        public async Task<User?> GetByUserNameAsync(string userName, CancellationToken ct = default)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower(), ct);

            return user;
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), ct);

            return user;
        }

        public async Task<bool> IsExistsUserNameAsync(string userName, CancellationToken ct = default)
        {
            return await _context.Users
                .AnyAsync(u => u.UserName.ToLower() == userName.ToLower(), ct);
        }

        public async Task<bool> IsExistsEmailAsync(string email, CancellationToken ct = default)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower(), ct);
        }
    }
}
