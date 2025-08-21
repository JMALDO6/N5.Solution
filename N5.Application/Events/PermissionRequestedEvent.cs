namespace N5.Application.Events
{
    public class PermissionRequestedEvent
    {
        public int PermissionId { get; set; }
        public int EmployeeId { get; set; }
        public int PermissionType { get; set; }
        public DateTime Timestamp { get; set; }

        // (Opcional) Versionado
        public string Version => "v1";

    }
}
