using System;
using System.Collections.Generic;
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

namespace DeliveryTracking.Application.Features.Orders.Commands.CompleteDelivery
{
    public class CompleteDeliveryCommandHandler : IRequestHandler<CompleteDeliveryCommand, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<CompleteDeliveryCommandHandler> _logger;

        public CompleteDeliveryCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ILogger<CompleteDeliveryCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<Result<OrderDto>> Handle(CompleteDeliveryCommand request, CancellationToken cancellationToken)
        {
            // Verify current user is authenticated
            if (!_currentUserService.IsAuthenticated || _currentUserService.UserId == null)
            {
                return Result<OrderDto>.FailureResult(
                    "Unauthorized",
                    new List<string> { "User is not authenticated." }
                );
            }

            // Validate order exists
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                return Result<OrderDto>.FailureResult(
                    "Order not found",
                    new List<string> { "The specified order does not exist." }
                );
            }

            // Validate rider is assigned to this order
            if (order.AssignedRiderId != _currentUserService.UserId)
            {
                return Result<OrderDto>.FailureResult(
                    "Unauthorized",
                    new List<string> { "You are not assigned to this order." }
                );
            }

            // Validate order status is PickedUp or OnTheWay
            if (order.Status != OrderStatus.PickedUp && order.Status != OrderStatus.OnTheWay)
            {
                return Result<OrderDto>.FailureResult(
                    "Invalid order status",
                    new List<string> { "Only picked up or on the way orders can be delivered." }
                );
            }

            // Update order status and delivery time
            order.Status = OrderStatus.Delivered;
            order.DeliveryTime = DateTime.UtcNow;
            _unitOfWork.Orders.Update(order);

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order {OrderId} delivered by Rider {RiderId}. OrderNumber: {OrderNumber}", 
                request.OrderId, _currentUserService.UserId, order.OrderNumber);

            // Map to DTO and return
            var orderDto = MapToOrderDto(order);
            return Result<OrderDto>.SuccessResult(orderDto, "Order delivered successfully");
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
