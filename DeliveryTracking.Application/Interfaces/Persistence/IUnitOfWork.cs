using System;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracking.Domain.Entities;

namespace DeliveryTracking.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<AppUser> Users { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<DeliveryAssignment> DeliveryAssignments { get; }
        IGenericRepository<RiderLocation> RiderLocations { get; }
        IGenericRepository<Notification> Notifications { get; }
        IGenericRepository<UserDeviceToken> UserDeviceTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
