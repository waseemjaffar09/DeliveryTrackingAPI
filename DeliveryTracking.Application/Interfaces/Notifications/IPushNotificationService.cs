namespace DeliveryTracking.Application.Interfaces.Notifications
{
    public interface IPushNotificationService
    {
        Task SendToUserAsync(
            Guid userId,
            string title,
            string body,
            Dictionary<string, string>? data = null);

        Task SendToUsersAsync(
            IEnumerable<Guid> userIds,
            string title,
            string body,
            Dictionary<string, string>? data = null);

        Task SendToTopicAsync(
            string topic,
            string title,
            string body,
            Dictionary<string, string>? data = null);
    }
}
