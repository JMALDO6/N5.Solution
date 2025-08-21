using N5.Domain.Entities;

namespace N5.Domain.Interfaces
{
    /// <summary>
    /// Permission repository interface for managing permissions in the data source.
    /// </summary>
    public interface IPermissionRepository
    {
        /// <summary>
        /// Gets all permissions from the data source asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Permission>> GetAllAsync();

        /// <summary>
        /// Gets a permission by its identifier asynchronously.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Permission> GetByIdAsync(int id);

        /// <summary>
        /// Adds a new permission to the data source asynchronously.
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        Task AddAsync(Permission permission);

        /// <summary>
        /// Updates an existing permission in the data source.
        /// </summary>
        /// <param name="permission"></param>
        void Update(Permission permission);

        /// <summary>
        /// Saves all changes made in the repository to the data source asynchronously.
        /// </summary>
        /// <returns></returns>
        Task SaveAsync();

    }
}
