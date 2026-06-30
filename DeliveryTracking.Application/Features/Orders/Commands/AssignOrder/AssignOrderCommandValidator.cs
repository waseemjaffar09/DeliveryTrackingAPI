using FluentValidation;
using DeliveryTracking.Application.Features.Orders.Commands.AssignOrder;

namespace DeliveryTracking.Application.Features.Orders.Commands.AssignOrder
{
    public class AssignOrderCommandValidator : AbstractValidator<AssignOrderCommand>
    {
        public AssignOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID is required");

            RuleFor(x => x.RiderId)
                .NotEmpty().WithMessage("Rider ID is required");
        }
    }
}
