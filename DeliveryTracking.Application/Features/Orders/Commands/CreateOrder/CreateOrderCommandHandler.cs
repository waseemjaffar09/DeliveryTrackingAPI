using System;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Orders.DTOs;
using DeliveryTracking.Application.Interfaces.Identity;
using DeliveryTracking.Application.Interfaces.Persistence;
using DeliveryTracking.Domain.Entities;
using DeliveryTracking.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryTracking.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CreateOrderCommandHandler> _logger;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<CreateOrderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            // Verify current user is authenticated and is a manager
            if (!_currentUserService.IsAuthenticated || _currentUserService.UserId == null)
            {
                return Result<OrderDto>.FailureResult(
                    "Unauthorized",
                    new List<string> { "User is not authenticated." }
                );
            }

            if (_currentUserService.Role != UserRole.Manager)
            {
                return Result<OrderDto>.FailureResult(
                    "Forbidden",
                    new List<string> { "Only managers can create orders." }
                );
            }

            // Check if order number already exists
            var existingOrders = await _unitOfWork.Orders.GetAllAsync(cancellationToken);
            if (existingOrders.Any(o => o.OrderNumber == request.OrderNumber))
            {
                return Result<OrderDto>.FailureResult(
                    "Order number already exists",
                    new List<string> { "An order with this order number already exists." }
                );
            }

            // Create new order
            var newOrder = new Order
            {
                OrderNumber = request.OrderNumber,
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Status = OrderStatus.Pending,
                CreatedByManagerId = _currentUserService.UserId.Value
            };

            // Add order to repository
            await _unitOfWork.Orders.AddAsync(newOrder, cancellationToken);

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order {OrderId} created with OrderNumber {OrderNumber} by Manager {ManagerId}", 
                newOrder.Id, newOrder.OrderNumber, _currentUserService.UserId);

            // Map to DTO and return
            var orderDto = MapToOrderDto(newOrder);
            return Result<OrderDto>.SuccessResult(orderDto, "Order created successfully");
        }

        private OrderDto MapToOrderDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                Address = order.Address,
                Latitude = order.Latitude,
                Longitude = order.Longitude,
                Status = order.Status,
                AssignedRiderId = order.AssignedRiderId,
                CreatedByManagerId = order.CreatedByManagerId,
                PickupTime = order.PickupTime,
                DeliveryTime = order.DeliveryTime,
                CreatedAt = order.CreatedAt
            };
        }
    }
}
