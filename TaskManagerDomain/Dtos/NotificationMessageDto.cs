namespace TaskManagerMessaging.Messaging
{
    public class NotificationMessageDto
    {
        public Guid UserId { get; set; }
        public Guid TaskId { get; set; }
        public string Message { get; set; }
    }
}
