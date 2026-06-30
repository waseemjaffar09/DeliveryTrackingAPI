using FluentValidation;
using DeliveryTracking.Application.Features.Auth.Commands.Register;

namespace DeliveryTracking.Application.Features.Auth.Commands.Register
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(150).WithMessage("Full name must not exceed 150 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be valid")
                .MaximumLength(150).WithMessage("Email must not exceed 150 characters");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(r => r == 1 || r == 2).WithMessage("Role must be either Manager (1) or Rider (2)");
        }
    }
}
