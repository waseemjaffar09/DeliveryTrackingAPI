using FluentValidation;
using DeliveryTracking.Application.Features.Locations.Commands.UpdateRiderLocation;

namespace DeliveryTracking.Application.Features.Locations.Commands.UpdateRiderLocation
{
    public class UpdateRiderLocationCommandValidator : AbstractValidator<UpdateRiderLocationCommand>
    {
        public UpdateRiderLocationCommandValidator()
        {
            RuleFor(x => x.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");

            RuleFor(x => x.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180");
        }
    }
}
