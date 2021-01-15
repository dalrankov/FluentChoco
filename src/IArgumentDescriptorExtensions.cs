using HotChocolate.Types;

namespace FluentChoco
{
    public static class IArgumentDescriptorExtensions
    {
        /// <summary>
        /// Skip object validation.
        /// </summary>
        public static IArgumentDescriptor SkipValidation(
            this IArgumentDescriptor descriptor)
        {
            descriptor.Extend().OnBeforeCreate(
                d => d.ContextData[ArgumentValidationKeys.Skip] = true);

            return descriptor;
        }

        /// <summary>
        /// Validate the specified rule sets only. 
        /// By default, only unspecified "default" rule set is being validated. 
        /// To validate all rule sets pass "*".
        /// </summary>
        public static IArgumentDescriptor ValidateRuleSets(
            this IArgumentDescriptor descriptor,
            params string[] ruleSets)
        {
            descriptor.Extend().OnBeforeCreate(
                d => d.ContextData[ArgumentValidationKeys.RuleSets] = ruleSets);

            return descriptor;
        }

        /// <summary>
        /// Validate the specified properties only. 
        /// This will inspect all rule sets in search for the specified property names.
        /// </summary>
        public static IArgumentDescriptor ValidateProperties(
            this IArgumentDescriptor descriptor,
            params string[] properties)
        {
            descriptor.Extend().OnBeforeCreate(
                d => d.ContextData[ArgumentValidationKeys.Properties] = properties);

            return descriptor;
        }
    }
}