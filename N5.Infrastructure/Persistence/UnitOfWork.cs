using N5.Domain.Interfaces;

namespace N5.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PermissionsDbContext _context;
        public IPermissionRepository Permissions { get; }

        public UnitOfWork(PermissionsDbContext context, IPermissionRepository permissions)
        {
            _context = context;
            Permissions = permissions;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
