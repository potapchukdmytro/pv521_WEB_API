using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Role> Roles => GetAll();

        public async Task<bool> IsExistsAsync(string name, CancellationToken ct = default)
        {
            return await Roles.AnyAsync(r => r.Name.ToLower() == name.ToLower(), ct);
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return await Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == name.ToLower(), ct);
        }
    }
}
