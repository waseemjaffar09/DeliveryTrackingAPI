using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace DeliveryTracking.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Run all validators
            var validationTasks = _validators.Select(v => v.ValidateAsync(request, cancellationToken));
            var validationResults = await Task.WhenAll(validationTasks);

            // Collect all errors
            var errors = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .Select(f => f.ErrorMessage)
                .ToList();

            if (errors.Any())
            {
                throw new ValidationException("Validation failed", validationResults.SelectMany(r => r.Errors));
            }

            // Proceed to next behavior/handler
            return await next();
        }
    }
}
