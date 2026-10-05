using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly AppDbContext _context;
        private readonly RoleRepository _roleRepository;

        public UserRepository(AppDbContext context, RoleRepository roleRepository)
            : base(context)
        {
            _context = context;
            _roleRepository = roleRepository;
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

        public async Task<string> GetUserRoleAsync(User user, CancellationToken ct = default)
        {
            await _context.Entry(user).Reference(u => u.Role).LoadAsync(ct);
            return user.Role == null ? "user" : user.Role.Name;
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

        public async Task<bool> AddToRoleAsync(User user, string roleName, CancellationToken ct = default)
        {
            var role = await _roleRepository.GetByNameAsync(roleName, ct);

            if(role == null)
            {
                return false;
            }

            if(user.RoleId != role.Id)
            {
                user.RoleId = role.Id;
                await UpdateAsync(user, ct);
            }

            return true;
        }
    }
}
