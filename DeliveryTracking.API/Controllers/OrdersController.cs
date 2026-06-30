using System;
using System.Threading.Tasks;
using DeliveryTracking.Application.Features.Orders.Commands.AcceptOrder;
using DeliveryTracking.Application.Features.Orders.Commands.AssignOrder;
using DeliveryTracking.Application.Features.Orders.Commands.CompleteDelivery;
using DeliveryTracking.Application.Features.Orders.Commands.CreateOrder;
using DeliveryTracking.Application.Features.Orders.Commands.PickupOrder;
using DeliveryTracking.Application.Features.Orders.DTOs;
using DeliveryTracking.Application.Features.Orders.Queries.GetOrderById;
using DeliveryTracking.Application.Features.Orders.Queries.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryTracking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            var command = new CreateOrderCommand
            {
                OrderNumber = request.OrderNumber,
                CustomerName = request.CustomerName,
                CustomerPhone = request.CustomerPhone,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude
            };

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return CreatedAtAction(nameof(GetOrderById), new { id = result.Data?.Id }, result.Data);
            }

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }

        [HttpGet]
        [ProducesResponseType(typeof(System.Collections.Generic.IReadOnlyList<OrderDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrders()
        {
            var query = new GetOrdersQuery();
            var result = await _mediator.Send(query);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var query = new GetOrderByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return NotFound(new { message = result.Message, errors = result.Errors });
        }

        [HttpPut("{orderId}/assign-rider/{riderId}")]
        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignRider(Guid orderId, Guid riderId)
        {
            var command = new AssignOrderCommand
            {
                OrderId = orderId,
                RiderId = riderId
            };

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }

        [HttpPut("{orderId}/accept")]
        [Authorize(Policy = "RiderOnly")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AcceptOrder(Guid orderId)
        {
            var command = new AcceptOrderCommand
            {
                OrderId = orderId
            };

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }

        [HttpPut("{orderId}/pickup")]
        [Authorize(Policy = "RiderOnly")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PickupOrder(Guid orderId)
        {
            var command = new PickupOrderCommand
            {
                OrderId = orderId
            };

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }

        [HttpPut("{orderId}/complete")]
        [Authorize(Policy = "RiderOnly")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CompleteDelivery(Guid orderId)
        {
            var command = new CompleteDeliveryCommand
            {
                OrderId = orderId
            };

            var result = await _mediator.Send(command);

            if (result.Success)
            {
                return Ok(result.Data);
            }

            return BadRequest(new { message = result.Message, errors = result.Errors });
        }
    }
}
