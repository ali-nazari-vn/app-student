using MediatR;

namespace StudentApp.Application.CQRS.StudentNotification
{
    public class SendNotificationToStudent : INotification
    {
        public int UserId { get; set; }
        public string Message { get; set; }
    }

    public class AddRefreshTokenNotificationHandler : INotificationHandler<SendNotificationToStudent>
    {
        public AddRefreshTokenNotificationHandler()
        {
        }

        public async Task Handle(SendNotificationToStudent notification, CancellationToken cancellationToken)
        {
            // Send Notification To Student
        }
    }
}
