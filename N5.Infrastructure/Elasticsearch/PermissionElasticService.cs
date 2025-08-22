using N5.Domain.Entities;
using N5.Domain.Interfaces;
using Nest;

namespace N5.Infrastructure.Elasticsearch
{
    /// <summary>
    /// PermissionElasticService provides methods to interact with Elasticsearch for permission documents.
    /// </summary>
    public class PermissionElasticService : IPermissionElasticService
    {
        private readonly IElasticClient _elasticClient;
        private const string IndexName = "permissions";

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="elasticClient"></param>
        public PermissionElasticService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        /// <inheritdoc />
        public async Task IndexAsync(PermissionDocument document, CancellationToken cancellationToken)
        {
            await _elasticClient.IndexAsync(document, idx => idx.Index(IndexName), cancellationToken);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(PermissionDocument document, CancellationToken cancellationToken)
        {
            await _elasticClient.IndexAsync(document, idx => idx.Index(IndexName).Id(document.Id));
        }

        /// <inheritdoc />
        public async Task<List<PermissionDocument>> GetAllAsync()
        {
            var response = await _elasticClient.SearchAsync<PermissionDocument>(s => s
                .Index(IndexName)
                .MatchAll());

            return response.Documents.ToList();
        }
    }
}