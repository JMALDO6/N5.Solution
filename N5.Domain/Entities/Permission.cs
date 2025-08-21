namespace N5.Domain.Entities
{
    /// <summary>
    /// Permission entity representing an employee's permission request
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Identifier of the permission record
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee's forename requesting the permission
        /// </summary>
        public required string EmployeeForename { get; set; }

        /// <summary>
        /// Employee's surname requesting the permission
        /// </summary>
        public required string EmployeeSurname { get; set; }

        /// <summary>
        /// Permission type identifier
        /// </summary>
        public int PermissionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the permission was granted.
        /// </summary>
        public DateTime PermissionDate { get; set; }

        /// <summary>
        /// Permission type navigation property
        /// </summary>
        public virtual PermissionType PermissionType { get; set; }
    }

}
