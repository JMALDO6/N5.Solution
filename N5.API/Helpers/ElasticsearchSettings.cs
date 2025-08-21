namespace N5.API.Helpers
{
    /// <summary>
    /// Helper class to hold Elasticsearch settings.
    /// </summary>
    public class ElasticsearchSettings
    {
        /// <summary>
        /// URL of the Elasticsearch server.
        /// </summary>
        public string Uri { get; set; } = default!;

        /// <summary>
        /// Default index to use for Elasticsearch operations.
        /// </summary>
        public string DefaultIndex { get; set; } = default!;
    }

}
