using N5.Domain.Entities;

namespace N5.Domain.Interfaces
{
    public interface IPermissionElasticService
    {
        /// <summary>
        /// Index a new permission document in Elasticsearch
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task IndexAsync(PermissionDocument document, CancellationToken cancellationToken);

        /// <summary>
        /// Update an existing permission document in Elasticsearch
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(PermissionDocument document, CancellationToken cancellationToken);

        /// <summary>
        /// Get all permission documents from Elasticsearch
        /// </summary>
        /// <returns></returns>
        Task<List<PermissionDocument>> GetAllAsync();

        /// <summary>
        /// Get a permission document by its ID from Elasticsearch
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PermissionDocument?> GetPermissionByIdAsync(int id);
    }
}