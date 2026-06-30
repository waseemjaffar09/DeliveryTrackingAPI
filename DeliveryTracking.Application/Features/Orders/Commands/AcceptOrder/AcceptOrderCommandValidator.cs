using FluentValidation;
using DeliveryTracking.Application.Features.Orders.Commands.AcceptOrder;

namespace DeliveryTracking.Application.Features.Orders.Commands.AcceptOrder
{
    public class AcceptOrderCommandValidator : AbstractValidator<AcceptOrderCommand>
    {
        public AcceptOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID is required");
        }
    }
}
