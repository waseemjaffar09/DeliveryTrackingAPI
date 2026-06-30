namespace DeliveryTracking.Application.Interfaces.Realtime
{
    public interface IRealtimeNotificationService
    {
        Task SendRiderLocationToManagers(Guid riderId, decimal latitude, decimal longitude, DateTime updatedAt);
        Task SendOrderStatusUpdate(Guid orderId, string status);
    }
}
