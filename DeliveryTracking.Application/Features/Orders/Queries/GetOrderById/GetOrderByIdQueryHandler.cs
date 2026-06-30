using System;
using System.Threading;
using System.Threading.Tasks;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Orders.DTOs;
using DeliveryTracking.Application.Interfaces.Persistence;
using DeliveryTracking.Domain.Entities;
using MediatR;

namespace DeliveryTracking.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            // Get order by ID
            var order = await _unitOfWork.Orders.GetByIdAsync(request.OrderId, cancellationToken);

            if (order == null)
            {
                return Result<OrderDto>.FailureResult(
                    "Order not found",
                    new System.Collections.Generic.List<string> { "The requested order does not exist." }
                );
            }

            // Map to DTO and return
            var orderDto = MapToOrderDto(order);
            return Result<OrderDto>.SuccessResult(orderDto);
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
