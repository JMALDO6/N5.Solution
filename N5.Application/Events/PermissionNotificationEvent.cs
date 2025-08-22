using MediatR;

namespace N5.Application.Events
{
    /// <summary>
    /// PermissionRequestedEvent represents an event that is triggered when a permission request is made.
    /// </summary>
    public class PermissionNotificationEvent : INotification
    {
        /// <summary>
        /// PermissionId is the unique identifier for the permission request.
        /// </summary>
        public int PermissionId { get; set; }

        /// <summary>
        /// Operation is the type of operation being performed, such as "modify", "request", or "get".
        /// </summary>
        public required string Operation { get; set; }
    }
}
