using Microsoft.EntityFrameworkCore;
using N5.Domain.Entities;
using N5.Domain.Interfaces;

namespace N5.Infrastructure.Persistence
{
    /// <summary>
    /// Permission repository implementation for managing permissions in the data source.
    /// </summary>
    public class PermissionRepository : IPermissionRepository
    {
        /// <summary>
        /// Context for accessing the permissions database.
        /// </summary>
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
            await _context.Permissions.AddAsync(permission);

            return permission.Id;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Permission> GetByIdAsync(int id) => await _context.Permissions.FindAsync(id);

        /// <inheritdoc/>
        public async Task<Permission> Update(Permission permission)
        {
            _context.Permissions.Update(permission);
            await _context.SaveChangesAsync();
            
            return permission;
        }
    }
}
