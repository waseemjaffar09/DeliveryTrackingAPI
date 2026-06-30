using System;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracking.Application.Interfaces.Persistence;
using DeliveryTracking.Domain.Entities;
using DeliveryTracking.Persistence.Context;

namespace DeliveryTracking.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DeliveryTrackingDbContext _context;
        private IGenericRepository<AppUser>? _usersRepository;
        private IGenericRepository<Order>? _ordersRepository;
        private IGenericRepository<DeliveryAssignment>? _deliveryAssignmentsRepository;
        private IGenericRepository<RiderLocation>? _riderLocationsRepository;
        private IGenericRepository<Notification>? _notificationsRepository;
        private IGenericRepository<UserDeviceToken>? _userDeviceTokensRepository;

        public UnitOfWork(DeliveryTrackingDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<AppUser> Users => _usersRepository ??= new GenericRepository<AppUser>(_context);
        public IGenericRepository<Order> Orders => _ordersRepository ??= new GenericRepository<Order>(_context);
        public IGenericRepository<DeliveryAssignment> DeliveryAssignments => _deliveryAssignmentsRepository ??= new GenericRepository<DeliveryAssignment>(_context);
        public IGenericRepository<RiderLocation> RiderLocations => _riderLocationsRepository ??= new GenericRepository<RiderLocation>(_context);
        public IGenericRepository<Notification> Notifications => _notificationsRepository ??= new GenericRepository<Notification>(_context);
        public IGenericRepository<UserDeviceToken> UserDeviceTokens => _userDeviceTokensRepository ??= new GenericRepository<UserDeviceToken>(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
