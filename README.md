# <img align="center" src="https://raw.githubusercontent.com/dalrankov/FluentChoco/master/icon.png"/> FluentChoco

FluentValidation middleware for HotChocolate GraphQL

<a href="https://www.nuget.org/packages/FluentChoco"><img alt="NuGet Version" src="https://img.shields.io/nuget/v/FluentChoco"></a>
<a href="https://www.nuget.org/packages/FluentChoco"><img alt="NuGet Downloads" src="https://img.shields.io/nuget/dt/FluentChoco"></a>

## How it works?

Registered FluentValidation middleware will be executed on top of GraphQL field and validate every argument value with it's registered `IValidator<T>` implementation from IServiceProvider. Validation failures, if there are any, will be reported and displayed in the response and request execution will be terminated.

## Usage

There are two ways to add FluentValidation middleware: globally (for every field) and per-field.

Global:
````csharp
services.AddGraphQLServer()
    .UseFluentValidation();
````

Code first:
````csharp
descriptor.Field("demo")
    .UseFluentValidation();
````

Pure code first:
````csharp
[UseFluentValidation]
public string Demo() => "All good!";
````

Keep in mind that adding middleware both globally and per-field will end up in field arguments being validated twice.

## Validators registration

There is an easy way to automatically register all validators from your assembly.
Also, by specifying parameters you can modify their lifetime and choose whether you want to inspect only public (exported) types or rather all types.
By default, only public (exported) types will get inspected and validators will be registered with Transient lifetime.

````csharp
services.RegisterFluentValidators<Startup>();
````

## Validator customization

There are 3 ways to customize your validator's behaviour for each argument.
- Skip validation
- Specify the rule sets to validate
- Specify the properties to validate

Code first:
````csharp
descriptor.Field("demo")
    .Argument("input1", d => d.ValidateRuleSets("CustomRule"))
    .Argument("input2", d => d.ValidateProperties("Foo", "Bar"))
    .Argument("input3", d => d.SkipValidation());
````

Pure code first:

````csharp
public string Demo(
      [ValidateRuleSets("CustomRule")] TestInput input1,
      [ValidateProperties("Foo", "Bar")] TestInput input2,
      [SkipValidation] TestInput input3)
    {
        return "All good!";
    }
````

## Error customization

You can implement `IValidationErrorBuilder` to build your own errors.

````csharp
public class CustomErrorBuilder
    : IValidationErrorBuilder
{
    public IErrorBuilder BuildError(
        IErrorBuilder builder, 
        ValidationFailure failure, 
        IInputField argument, 
        IMiddlewareContext context)
    {
        return builder.SetCode("DEMO_ERROR")
            .SetMessage(failure.ErrorMessage)
            .SetExtension("custom1", "Hey, there was an error!");
    }
}
````

Global:
````csharp
services.AddGraphQLServer()
    .UseFluentValidation<CustomErrorBuilder>();
````

Code first:
````csharp
descriptor.Field("demo")
    .UseFluentValidation<CustomErrorBuilder>();
````

Pure code first:
````csharp
[UseFluentValidation(typeof(CustomErrorBuilder))]
public string Demo() => "All good!";
````

------------------------
Icon made by [Ivana Vujačić](https://www.pinterest.com/vujacicnivana/).