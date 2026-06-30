using DeliveryTracking.Application.Common.Models;
using DeliveryTracking.Application.Common.Models;
using MediatR;

namespace DeliveryTracking.Application.Features.Notifications.Commands.RegisterDeviceToken
{
    public class RegisterDeviceTokenCommand : IRequest<Result<bool>>
    {
        public string Token { get; set; } = string.Empty;
        public string Platform { get; set; } = string.Empty;
    }
}
