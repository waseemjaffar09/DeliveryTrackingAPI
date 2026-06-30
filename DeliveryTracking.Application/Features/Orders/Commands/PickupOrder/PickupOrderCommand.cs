using System;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Orders.DTOs;
using MediatR;

namespace DeliveryTracking.Application.Features.Orders.Commands.PickupOrder
{
    public class PickupOrderCommand : IRequest<Result<OrderDto>>
    {
        public Guid OrderId { get; set; }
    }
}
