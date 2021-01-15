using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System;
using System.Reflection;

namespace FluentChoco
{
    /// <summary>
    /// Skip object validation.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class SkipValidationAttribute
        : ArgumentDescriptorAttribute
    {
        public override void OnConfigure(
            IDescriptorContext context,
            IArgumentDescriptor descriptor,
            ParameterInfo parameter)
        {
            descriptor.SkipValidation();
        }
    }
}