using Microsoft.AspNetCore.SignalR;

namespace DeliveryTracking.Infrastructure.SignalR
{
    public class TrackingHub : Hub
    {
        public const string ManagersGroup = "managers";

        public async Task JoinManagerGroup(Guid managerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, ManagersGroup);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"manager-{managerId}");
        }

        public async Task JoinRiderGroup(Guid riderId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"rider-{riderId}");
        }
    }
}
