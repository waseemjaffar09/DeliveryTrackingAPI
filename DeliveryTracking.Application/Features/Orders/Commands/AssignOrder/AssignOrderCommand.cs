using System;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Orders.DTOs;
using MediatR;

namespace DeliveryTracking.Application.Features.Orders.Commands.AssignOrder
{
    public class AssignOrderCommand : IRequest<Result<OrderDto>>
    {
        public Guid OrderId { get; set; }
        public Guid RiderId { get; set; }
    }
}
