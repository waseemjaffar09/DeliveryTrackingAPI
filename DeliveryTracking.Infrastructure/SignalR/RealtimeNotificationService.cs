using DeliveryTracking.Application.Interfaces.Realtime;
using Microsoft.AspNetCore.SignalR;

namespace DeliveryTracking.Infrastructure.SignalR
{
    public class RealtimeNotificationService : IRealtimeNotificationService
    {
        private readonly IHubContext<TrackingHub> _hubContext;

        public RealtimeNotificationService(IHubContext<TrackingHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendRiderLocationToManagers(Guid riderId, decimal latitude, decimal longitude, DateTime updatedAt)
        {
            await _hubContext.Clients.Group(TrackingHub.ManagersGroup).SendAsync(
                "RiderLocationUpdated",
                new
                {
                    RiderId = riderId,
                    Latitude = latitude,
                    Longitude = longitude,
                    UpdatedAt = updatedAt
                });
        }

        public async Task SendOrderStatusUpdate(Guid orderId, string status)
        {
            await _hubContext.Clients.Group(TrackingHub.ManagersGroup).SendAsync(
                "OrderStatusUpdated",
                new
                {
                    OrderId = orderId,
                    Status = status
                });
        }
    }
}
