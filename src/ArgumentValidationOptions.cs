using FluentValidation;
using FluentValidation.Internal;
using HotChocolate.Types;
using System;
using System.Linq;

namespace FluentChoco
{
    class ArgumentValidationOptions<TArgument>
    {
        readonly TArgument _instance;
        readonly bool? _skip;
        readonly string[] _ruleSets;
        readonly string[] _properties;

        public ArgumentValidationOptions(
            IInputField argument,
            TArgument instance)
        {
            _instance = instance;
            _skip = argument.ContextData.GetValueOrDefault(ArgumentValidationKeys.Skip) as bool?;
            _ruleSets = argument.ContextData.GetValueOrDefault(ArgumentValidationKeys.RuleSets) as string[];
            _properties = argument.ContextData.GetValueOrDefault(ArgumentValidationKeys.Properties) as string[];
        }

        public bool AllowedToValidate()
        {
            return _instance != null && _skip != true;
        }

        public ValidationContext<TArgument> BuildValidationContext()
        {
            return ValidationContext<TArgument>.CreateWithOptions(
                _instance, CustomizeValidationStrategy);
        }

        public IValidator GetValidator(
            IServiceProvider serviceProvider)
        {
            return serviceProvider.GetService(
                typeof(IValidator<>).MakeGenericType(_instance.GetType())) as IValidator;
        }

        void CustomizeValidationStrategy(
            ValidationStrategy<TArgument> strategy)
        {
            if (_ruleSets?.Any() == true)
            {
                strategy.IncludeRuleSets(_ruleSets);
            }
            else if (_properties?.Any() == true)
            {
                strategy.IncludeProperties(_properties);
            }
        }
    }
}