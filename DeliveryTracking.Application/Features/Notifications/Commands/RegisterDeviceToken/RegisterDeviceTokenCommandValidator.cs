using FluentValidation;
using FluentValidation;

namespace DeliveryTracking.Application.Features.Notifications.Commands.RegisterDeviceToken
{
    public class RegisterDeviceTokenCommandValidator : AbstractValidator<RegisterDeviceTokenCommand>
    {
        public RegisterDeviceTokenCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Device token is required")
                .MaximumLength(500).WithMessage("Device token must not exceed 500 characters");

            RuleFor(x => x.Platform)
                .NotEmpty().WithMessage("Platform is required")
                .MaximumLength(50).WithMessage("Platform must not exceed 50 characters");
        }
    }
}
