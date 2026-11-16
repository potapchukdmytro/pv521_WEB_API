using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public const string UniqueNameIndex = "IX_Roles_Name_CaseInsensitive";

        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Role> Roles => GetAll();

        public async Task<bool> IsExistsAsync(
            string name, CancellationToken ct = default)
        {
            return await GetByNameAsync(name, ct) != null;
        }

        public async Task<Role?> GetByNameAsync(
            string name, CancellationToken ct = default)
        {
            return await Roles.FirstOrDefaultAsync(
                role => role.Name.ToLower() == name.Trim().ToLower(), ct);
        }
    }
}
