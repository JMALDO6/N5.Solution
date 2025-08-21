using N5.Domain.Interfaces;

namespace N5.Infrastructure.Persistence
{
    /// <summary>
    /// UnitOfWork implements the IUnitOfWork interface to manage repositories and commit changes.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// Context for database operations.
        /// </summary>
        private readonly PermissionsDbContext _context;

        /// <inheritdoc />
        public IPermissionRepository Permissions { get; }

        /// <summary>
        /// Constructor for UnitOfWork.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="permissions"></param>
        public UnitOfWork(PermissionsDbContext context, IPermissionRepository permissions)
        {
            _context = context;
            Permissions = permissions;
        }

        /// <inheritdoc />
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
