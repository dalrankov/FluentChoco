using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System;
using System.Reflection;

namespace FluentChoco
{
    /// <summary>
    /// Adds FluentValidation field middleware.
    /// It is going to validate all non-null field arguments and report errors if there is one or more failures.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public sealed class UseFluentValidationAttribute
        : ObjectFieldDescriptorAttribute
    {
        readonly Type _errorBuilderType;

        public UseFluentValidationAttribute()
        {
        }

        /// <param name="errorBuilderType">Custom error builder class type used to construct single validation error.</param>
        public UseFluentValidationAttribute(
            Type errorBuilderType)
        {
            if (!(typeof(IValidationErrorBuilder).IsAssignableFrom(errorBuilderType) && errorBuilderType.IsClass))
            {
                throw new ArgumentException($"{errorBuilderType.Name} is not a class implementing {nameof(IValidationErrorBuilder)}!");
            }

            _errorBuilderType = errorBuilderType;
        }

        public override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            if (_errorBuilderType != null)
            {
                descriptor.UseFluentValidation(_errorBuilderType);
            }
            else
            {
                descriptor.UseFluentValidation();
            }
        }
    }
}