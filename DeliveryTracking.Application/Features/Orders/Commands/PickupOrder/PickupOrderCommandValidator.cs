using FluentValidation;
using DeliveryTracking.Application.Features.Orders.Commands.PickupOrder;

namespace DeliveryTracking.Application.Features.Orders.Commands.PickupOrder
{
    public class PickupOrderCommandValidator : AbstractValidator<PickupOrderCommand>
    {
        public PickupOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID is required");
        }
    }
}
