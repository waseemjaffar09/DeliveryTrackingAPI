using FluentValidation;

namespace DeliveryTracking.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Access token is required")
                .Must(token => !string.IsNullOrWhiteSpace(token)).WithMessage("Access token cannot be empty");

            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required")
                .Must(token => !string.IsNullOrWhiteSpace(token)).WithMessage("Refresh token cannot be empty");
        }
    }
}
