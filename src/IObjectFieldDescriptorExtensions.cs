using HotChocolate.Types;
using System;

namespace FluentChoco
{
    public static class IObjectFieldDescriptorExtensions
    {
        /// <summary>
        /// Adds FluentValidation field middleware.
        /// It is going to validate all non-null field arguments and report errors if there is one or more failures.
        /// </summary>
        public static IObjectFieldDescriptor UseFluentValidation(
            this IObjectFieldDescriptor descriptor)
        {
            return descriptor.UseFluentValidation<ValidationErrorBuilder>();
        }

        /// <summary>
        /// Adds FluentValidation field middleware.
        /// It is going to validate all non-null field arguments and report errors if there is one or more failures.
        /// </summary>
        /// <typeparam name="T">Custom error builder class used to construct single validation error.</typeparam>
        public static IObjectFieldDescriptor UseFluentValidation<TErrorBuilder>(
            this IObjectFieldDescriptor descriptor) where TErrorBuilder : IValidationErrorBuilder
        {
            return descriptor.UseFluentValidation(typeof(TErrorBuilder));
        }

        /// <summary>
        /// Adds FluentValidation field middleware.
        /// It is going to validate all non-null field arguments and report errors if there is one or more failures.
        /// </summary>
        /// <param name="errorBuilderType">Custom error builder class type used to construct single validation error.</param>
        public static IObjectFieldDescriptor UseFluentValidation(
            this IObjectFieldDescriptor descriptor,
            Type errorBuilderType)
        {
            if (!(typeof(IValidationErrorBuilder).IsAssignableFrom(errorBuilderType) && errorBuilderType.IsClass))
            {
                throw new ArgumentException($"{errorBuilderType.Name} is not a class implementing {nameof(IValidationErrorBuilder)}!");
            }

            return descriptor.Use((provider, next) =>
                new FluentValidationMiddleware(next, errorBuilderType));
        }
    }
}