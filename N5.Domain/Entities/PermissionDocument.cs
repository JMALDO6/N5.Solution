namespace N5.Domain.Entities
{
    /// <summary>
    /// PermissionDocument represents the structure of a permission document stored in Elasticsearch.
    /// </summary>
    public class PermissionDocument
    {
        /// <summary>
        /// ID of the permission document.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee's forename associated with the permission.
        /// </summary>
        public string EmployeeForename { get; set; }

        /// <summary>
        /// Employee's surname associated with the permission.
        /// </summary>
        public string EmployeeSurname { get; set; }

        /// <summary>
        /// Permission type ID indicating the type of permission granted.
        /// </summary>
        public int PermissionTypeId { get; set; }

        /// <summary>
        /// Permission date indicating when the permission was granted.
        /// </summary>
        public DateTime PermissionDate { get; set; }
    }

}
