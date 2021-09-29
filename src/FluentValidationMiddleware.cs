using FluentValidation;
using FluentValidation.Results;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentChoco
{
    class FluentValidationMiddleware
    {
        readonly FieldDelegate _next;
        readonly Type _errorBuilderType;

        public FluentValidationMiddleware(
            FieldDelegate next,
            Type errorBuilderType)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _errorBuilderType = errorBuilderType ?? throw new ArgumentNullException(nameof(errorBuilderType));
        }

        public async Task Invoke(
            IMiddlewareContext context)
        {
            if (context.Selection.SyntaxNode.Arguments.Any())
            {
                var serviceProvider = context.Service<IServiceProvider>();
                var errors = new List<(IInputField Argument, IList<ValidationFailure> Failures)>();

                foreach (IInputField argument in context.Selection.Field.Arguments)
                {
                    var validationOptions = new ArgumentValidationOptions<object>(
                        argument, context.ArgumentValue<object>(argument.Name));

                    if (!validationOptions.AllowedToValidate())
                    {
                        continue;
                    }

                    IValidator validator = validationOptions.GetValidator(serviceProvider);

                    if (validator == null)
                    {
                        continue;
                    }

                    ValidationResult result = await validator.ValidateAsync(
                        validationOptions.BuildValidationContext(), context.RequestAborted).ConfigureAwait(false);

                    if (result.Errors.Any())
                    {
                        errors.Add((argument, result.Errors));
                    }
                }

                if (errors.Any())
                {
                    var errorBuilder = ActivatorUtilities.CreateInstance(
                        serviceProvider, _errorBuilderType) as IValidationErrorBuilder;

                    foreach (var error in errors)
                    {
                        foreach (var failure in error.Failures)
                        {
                            context.ReportError(
                                errorBuilder.BuildError(
                                    ErrorBuilder.New(), failure, error.Argument, context)
                                .Build());
                        }
                    }

                    return;
                }
            }

            await _next(context).ConfigureAwait(false);
        }
    }
}