using FluentValidation;
using DeliveryTracking.Application.Features.Orders.Commands.CompleteDelivery;

namespace DeliveryTracking.Application.Features.Orders.Commands.CompleteDelivery
{
    public class CompleteDeliveryCommandValidator : AbstractValidator<CompleteDeliveryCommand>
    {
        public CompleteDeliveryCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("Order ID is required");
        }
    }
}
