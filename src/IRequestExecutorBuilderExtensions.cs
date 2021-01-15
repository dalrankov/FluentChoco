using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentChoco
{
    public static class IRequestExecutorBuilderExtensions
    {
        /// <summary>
        /// Adds FluentValidation global field middleware.
        /// It is going to validate all non-null field arguments and report errors if there is one or more failures.
        /// </summary>
        public static IRequestExecutorBuilder UseFluentValidation(
            this IRequestExecutorBuilder builder)
        {
            return builder.UseFluentValidation<ValidationErrorBuilder>();
        }

        /// <summary>
        /// Adds FluentValidation global field middleware.
        /// It is going to validate all non-null field arguments and report errors if there is one or more failures.
        /// </summary>
        /// <typeparam name="T">Custom error builder class used to construct single validation error.</typeparam>
        public static IRequestExecutorBuilder UseFluentValidation<TErrorBuilder>(
            this IRequestExecutorBuilder builder) where TErrorBuilder : IValidationErrorBuilder
        {
            return builder.UseFluentValidation(typeof(TErrorBuilder));
        }

        /// <summary>
        /// Adds FluentValidation global field middleware.
        /// It is going to validate all non-null field arguments and report errors if there is one or more failures.
        /// </summary>
        /// <param name="errorBuilderType">Custom error builder class type used to construct single validation error.</param>
        public static IRequestExecutorBuilder UseFluentValidation(
            this IRequestExecutorBuilder builder,
            Type errorBuilderType)
        {
            if (!(typeof(IValidationErrorBuilder).IsAssignableFrom(errorBuilderType) && errorBuilderType.IsClass))
            {
                throw new ArgumentException($"{errorBuilderType.Name} is not a class implementing {nameof(IValidationErrorBuilder)}!");
            }

            return builder.UseField((provider, next) =>
                new FluentValidationMiddleware(next, errorBuilderType));
        }
    }
}