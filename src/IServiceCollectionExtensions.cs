using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace FluentChoco
{
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Registers all FluentValidation validator classes within an assembly as implementations of their <see cref="IValidator{T}"/> declarations.
        /// Only non-generic and non-abstract classes are being inspected.
        /// </summary>
        /// <typeparam name="TContaining">Any type contained inside assembly you want to use for registration of validators.</typeparam>
        /// <param name="includeAllTypes">Indicates should all assembly types be inspected for registration. By default, only public (exported) types are being inspected, so - false.</param>
        /// <param name="validatorsLifetime">Lifetime for each <see cref="IValidator{T}"/> instance created from <see cref="IServiceProvider"/>. Default is <see cref="ServiceLifetime.Transient"/>.</param>
        public static IServiceCollection RegisterFluentValidators<TContaining>(
           this IServiceCollection services,
           bool includeAllTypes = false,
           ServiceLifetime validatorsLifetime = ServiceLifetime.Transient) where TContaining : class
        {
            return services.RegisterFluentValidators(
                typeof(TContaining).Assembly, includeAllTypes, validatorsLifetime);
        }

        /// <summary>
        /// Registers all FluentValidation validator classes within an assembly as implementations of their <see cref="IValidator{T}"/> declarations.
        /// Only non-generic and non-abstract classes are being inspected.
        /// </summary>
        /// <param name="assembly">Assembly you want to use for registration of validators.</param>
        /// <param name="includeAllTypes">Indicates should all assembly types be inspected for registration. By default, only public (exported) types are being inspected, so - false.</param>
        /// <param name="validatorsLifetime">Lifetime for each <see cref="IValidator{T}"/> instance created from <see cref="IServiceProvider"/>. Default is <see cref="ServiceLifetime.Transient"/>.</param>
        public static IServiceCollection RegisterFluentValidators(
           this IServiceCollection services,
           Assembly assembly,
           bool includeAllTypes = false,
           ServiceLifetime validatorsLifetime = ServiceLifetime.Transient)
        {
            Type[] types = includeAllTypes ?
                assembly.GetTypes() :
                assembly.GetExportedTypes();

            foreach (Type type in types)
            {
                if (typeof(IValidator).IsAssignableFrom(type)
                    && type.IsClass
                    && !type.IsAbstract
                    && !type.IsGenericTypeDefinition)
                {
                    foreach (Type interfaceType in type.GetInterfaces())
                    {
                        if (interfaceType.IsGenericType
                            && interfaceType.GetGenericTypeDefinition() == typeof(IValidator<>))
                        {
                            services.Add(new ServiceDescriptor(
                                interfaceType, type, validatorsLifetime));
                        }
                    }
                }
            }

            return services;
        }
    }
}