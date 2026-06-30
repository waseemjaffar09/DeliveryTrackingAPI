using System;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Orders.DTOs;
using MediatR;

namespace DeliveryTracking.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Result<OrderDto>>
    {
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
