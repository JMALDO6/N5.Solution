using N5.Domain.Entities;
using N5.Domain.Interfaces;
using N5.Infrastructure.Persistence;

namespace N5.Infrastructure.Repositories
{
    /// <summary>
    /// Permission repository implementation for managing permissions in the data source.
    /// </summary>
    public class PermissionRepository : IPermissionRepository
    {
        private readonly PermissionsDbContext _context;

        /// <summary>
        /// Constructor for PermissionRepository.
        /// </summary>
        /// <param name="context"></param>
        public PermissionRepository(PermissionsDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public async Task<int> AddAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            return permission.Id;
        }

        /// <inheritdoc/>
        public Task<IEnumerable<Permission>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<Permission?> GetByIdAsync(int id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        /// <inheritdoc/>
        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void Update(Permission permission)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        Task IPermissionRepository.AddAsync(Permission permission)
        {
            return AddAsync(permission);
        }
    }
}
