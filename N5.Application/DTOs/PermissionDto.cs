namespace N5.Application.DTOs
{
    /// <summary>
    /// Permission Data Transfer Object (DTO)
    /// </summary>
    public class PermissionDto
    {
        /// <summary>
        /// Identifier of the permission record
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Employee's full name requesting the permission
        /// </summary>
        public required string EmployeeName { get; set; }

        /// <summary>
        /// Employee's last name requesting the permission
        /// </summary>
        public required string EmployeeLastName { get; set; }

        /// <summary>
        /// Permission type identifier
        /// </summary>
        public int PermissionTypeId { get; set; }

        /// <summary>
        /// Permission date
        /// </summary>
        public DateTime PermissionDate { get; set; }
    }
}
