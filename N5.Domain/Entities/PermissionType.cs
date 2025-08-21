namespace N5.Domain.Entities
{
    /// <summary>
    /// PermissionType entity representing the type of permission
    /// </summary>
    public class PermissionType
    {
        /// <summary>
        /// Identifier of the permission type
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Description of the permission type
        /// </summary>
        public required string Description { get; set; }
    }
}
