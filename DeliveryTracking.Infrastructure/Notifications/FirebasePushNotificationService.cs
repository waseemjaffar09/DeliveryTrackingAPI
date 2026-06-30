using DeliveryTracking.Application.Interfaces.Notifications;
using Microsoft.Extensions.Logging;

namespace DeliveryTracking.Infrastructure.Notifications
{
    public class FirebasePushNotificationService : IPushNotificationService
    {
        private readonly FirebaseSettings _firebaseSettings;
        private readonly ILogger<FirebasePushNotificationService> _logger;

        public FirebasePushNotificationService(
            FirebaseSettings firebaseSettings,
            ILogger<FirebasePushNotificationService> logger)
        {
            _firebaseSettings = firebaseSettings;
            _logger = logger;
        }

        public Task SendToUserAsync(
            Guid userId,
            string title,
            string body,
            Dictionary<string, string>? data = null)
        {
            LogNotificationRequest("SendToUser", title, body, data, userId: userId);

            // TODO: Resolve user FCM token(s) from persistence, then call SendFirebaseMessageAsync.
            throw new NotImplementedException(
                "Firebase push notifications are not configured. Implement SendToUserAsync using the Firebase Admin SDK.");
        }

        public Task SendToUsersAsync(
            IEnumerable<Guid> userIds,
            string title,
            string body,
            Dictionary<string, string>? data = null)
        {
            var userIdList = userIds.ToList();
            LogNotificationRequest("SendToUsers", title, body, data, userIds: userIdList);

            // TODO: Resolve FCM tokens for each user, then batch-send via Firebase Admin SDK.
            throw new NotImplementedException(
                "Firebase push notifications are not configured. Implement SendToUsersAsync using the Firebase Admin SDK.");
        }

        public Task SendToTopicAsync(
            string topic,
            string title,
            string body,
            Dictionary<string, string>? data = null)
        {
            LogNotificationRequest("SendToTopic", title, body, data, topic: topic);

            // TODO: Send to Firebase topic, e.g. FirebaseMessaging.DefaultInstance.SendAsync(message).
            throw new NotImplementedException(
                "Firebase push notifications are not configured. Implement SendToTopicAsync using the Firebase Admin SDK.");
        }

        private void LogNotificationRequest(
            string method,
            string title,
            string body,
            Dictionary<string, string>? data,
            Guid? userId = null,
            IReadOnlyList<Guid>? userIds = null,
            string? topic = null)
        {
            _logger.LogInformation(
                "Push notification stub invoked. Method={Method}, ProjectId={ProjectId}, UserId={UserId}, UserCount={UserCount}, Topic={Topic}, Title={Title}, Body={Body}, DataKeys={DataKeys}",
                method,
                _firebaseSettings.ProjectId,
                userId,
                userIds?.Count,
                topic,
                title,
                body,
                data is null ? null : string.Join(',', data.Keys));
        }

        // Future Firebase integration entry point:
        // private async Task SendFirebaseMessageAsync(Message message, CancellationToken cancellationToken)
        // {
        //     EnsureFirebaseInitialized();
        //     await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
        // }
    }
}
