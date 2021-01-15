using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System;
using System.Reflection;

namespace FluentChoco
{
    /// <summary>
    /// Validate the specified rule sets only. 
    /// By default, only unspecified "default" rule set is being validated. 
    /// To validate all rule sets pass "*".
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class ValidateRuleSetsAttribute
        : ArgumentDescriptorAttribute
    {
        readonly string[] _ruleSets;

        /// <param name="ruleSets">The rule set names to validate.</param>
        public ValidateRuleSetsAttribute(
            params string[] ruleSets)
        {
            _ruleSets = ruleSets;
        }

        public override void OnConfigure(
            IDescriptorContext context,
            IArgumentDescriptor descriptor,
            ParameterInfo parameter)
        {
            descriptor.ValidateRuleSets(_ruleSets);
        }
    }
}