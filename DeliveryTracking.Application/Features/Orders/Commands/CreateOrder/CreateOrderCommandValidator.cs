using FluentValidation;
using DeliveryTracking.Application.Features.Orders.Commands.CreateOrder;

namespace DeliveryTracking.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty().WithMessage("Order number is required")
                .MaximumLength(100).WithMessage("Order number must not exceed 100 characters");

            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required")
                .MaximumLength(150).WithMessage("Customer name must not exceed 150 characters");

            RuleFor(x => x.CustomerPhone)
                .NotEmpty().WithMessage("Customer phone is required")
                .MaximumLength(50).WithMessage("Customer phone must not exceed 50 characters");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(500).WithMessage("Address must not exceed 500 characters");

            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90")
                .When(x => x.Latitude.HasValue);

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180")
                .When(x => x.Longitude.HasValue);
        }
    }
}
