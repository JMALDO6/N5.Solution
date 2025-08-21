namespace N5.Domain.Interfaces
{
    /// <summary>
    /// Interface for Unit of Work pattern to manage repositories and commit changes.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Permission repository instance.
        /// </summary>
        IPermissionRepository Permissions { get; }

        /// <summary>
        /// Saves all changes made in the context to the database asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangesAsync();
    }
}
