using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System;
using System.Reflection;

namespace FluentChoco
{
    /// <summary>
    /// Validate the specified properties only. 
    /// This will inspect all rule sets in search for the specified property names.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ValidatePropertiesAttribute
        : ArgumentDescriptorAttribute
    {
        readonly string[] _properties;

        /// <param name="properties">The property names to validate.</param>
        public ValidatePropertiesAttribute(
            params string[] properties)
        {
            _properties = properties;
        }

        public override void OnConfigure(
            IDescriptorContext context,
            IArgumentDescriptor descriptor,
            ParameterInfo parameter)
        {
            descriptor.ValidateProperties(_properties);
        }
    }
}