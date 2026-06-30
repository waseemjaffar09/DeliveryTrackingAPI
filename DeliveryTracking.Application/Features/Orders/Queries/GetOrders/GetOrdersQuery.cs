using System.Collections.Generic;
using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Features.Orders.DTOs;
using MediatR;

namespace DeliveryTracking.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<Result<IReadOnlyList<OrderDto>>>
    {
    }
}
