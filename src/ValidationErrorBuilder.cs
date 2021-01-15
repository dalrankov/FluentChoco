using FluentValidation.Results;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace FluentChoco
{
    class ValidationErrorBuilder
        : IValidationErrorBuilder
    {
        public IErrorBuilder BuildError(
            IErrorBuilder builder,
            ValidationFailure failure,
            IInputField argument,
            IMiddlewareContext context)
        {
            return builder.SetCode("VALIDATION_ERROR")
                .SetMessage(failure.ErrorMessage)
                .SetExtension("argument", argument.Name)
                .SetPath(context.Path);
        }
    }
}