using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Orders.DTOs;
using DeliveryTracking.Application.Interfaces.Persistence;
using DeliveryTracking.Domain.Entities;
using DeliveryTracking.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryTracking.Application.Features.Orders.Commands.AssignOrder
{
    public class AssignOrderCommandHandler : IRequestHandler<AssignOrderCommand, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignOrderCommandHandler> _logger;

        public AssignOrderCommandHandler(IUnitOfWork unitOfWork, ILogger<AssignOrderCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<OrderDto>> Handle(AssignOrderCommand request, CancellationToken cancellationToken)
        {
            // Validate order exists
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                return Result<OrderDto>.FailureResult(
                    "Order not found",
                    new List<string> { "The specified order does not exist." }
                );
            }

            // Validate rider exists
            var rider = await _unitOfWork.Users.GetByIdAsync(request.RiderId, cancellationToken);
            if (rider == null)
            {
                return Result<OrderDto>.FailureResult(
                    "Rider not found",
                    new List<string> { "The specified rider does not exist." }
                );
            }

            // Validate rider role is Rider
            if (rider.Role != UserRole.Rider)
            {
                return Result<OrderDto>.FailureResult(
                    "Invalid rider role",
                    new List<string> { "The specified user is not a rider." }
                );
            }

            // Validate order status is Pending
            if (order.Status != OrderStatus.Pending)
            {
                return Result<OrderDto>.FailureResult(
                    "Order cannot be assigned",
                    new List<string> { "Only pending orders can be assigned to riders." }
                );
            }

            // Assign rider to order
            order.AssignedRiderId = request.RiderId;
            order.Status = OrderStatus.Assigned;

            // Update order
            _unitOfWork.Orders.Update(order);

            // Create delivery assignment record
            var deliveryAssignment = new DeliveryAssignment
            {
                OrderId = request.OrderId,
                RiderId = request.RiderId,
                AssignedAt = DateTime.UtcNow
            };

            await _unitOfWork.DeliveryAssignments.AddAsync(deliveryAssignment, cancellationToken);

            // Save changes
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order {OrderId} assigned to Rider {RiderId}. OrderNumber: {OrderNumber}", 
                request.OrderId, request.RiderId, order.OrderNumber);

            // Map to DTO and return
            var orderDto = MapToOrderDto(order);
            return Result<OrderDto>.SuccessResult(orderDto, "Order assigned successfully");
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
