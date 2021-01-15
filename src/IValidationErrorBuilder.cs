using FluentValidation.Results;
using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace FluentChoco
{
    public interface IValidationErrorBuilder
    {
        IErrorBuilder BuildError(IErrorBuilder builder, ValidationFailure failure, IInputField argument, IMiddlewareContext context);
    }
}