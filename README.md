# ASP .NET Core Conditional Validations

Have custom `Required` attribute, which uses the validation context to decide
whether to apply itself or not - this is the server side validation.

The attribute would also have a property, which would be a JavaScript expression
for the client side and on the client side, the form elements which are created
and have their validation attributes set by ASP .NET Core would also be
decorated and there would be a hook which would run the JavaScript expression so
that they can also conditionally disable themselves based on the same rules.

A more systematic approach to this would be to not duplicate the expression,
once for the attribute `IsValid` override in C# and then for the client as the
JavaScript expression property which gets passed to the client, but is semantically
equivalent and instead have a DSL which generates the `IsValid` override logic as
well as the JavaScript expression for the client.

In case of the above DSL, instead of using attributes, Fluent Validation could
also be used and its rules configured from the declarative shared object and the
same object could be serialized for the client and the same object could be used
to generate the validation rules for jQuery validator or similar.

This way the validation rules would be only in once place and would stay in sync
across the backend and the frontend, enabling for conditional attributes based on
complex logic.

Whether to keep using data annotation attributes or Fluent Validations and to use
a plain JavaScript expression, jQuery validation, Kendo validator etc. on the client
would be a matter of choice as a "DSL backend" implementation.

## Example

Model:

```cs
class Model
{
  [Required]
  string Title { get; set; }
  
  [Required]
  string Category { get; set; }
  
  [RequiredIfCategory("category1")]
  string Category1Description { get; set; }
  

  [RequiredIfCategory("category2")]
  string Category2Description { get; set; }
}
```

This is using the data annotation attributes. `RequiredIfCategory` would be a custom
attribute with custom `IsValid` logic which would use the validation context from the
2nd argument to access the `Category` field on the model and either apply self or not
based on the field value.

```cs
class RequiredIfCategoryAttribute: RequiredAttribute
{
  public RequiredIfCategoryAttribute(string category)
  {
  
  }
  
  IsValid(object model, ValidationContext validationContext)
  {
    if (model.Category == _category) {
      // Required
    }
    else {
      // Not required
    }
  }
}
```

https://stackoverflow.com/a/16100455/2715716

This takes care of the backend, but on the frontend, the form element would still have
a required HTML attribute on itself, because this attribute still derives from `Required`
so that the ASP .NET Core validations framework would most likely still print that HTML
attribute since this custom attribute is based on the `Required` one.

To solve the client side, we would need to distinguish form elements which have been
marked with the required attribute based on the `Required` framework attribute or our
custom attribute.

If we could do that, we could run a JavaScript expression, which would find these
fields marked by the custom attribute and use the same logic which the attribute
implements on the backend side to do the same logic on the client side, resulting in
semantically equivalent validations on the server and on the client.

Of course this would mean duplicating the validation logic in C# and JS, so ideally,
there could be a DSL or a declarative validation ruleset object, which would be
executed by a generalized validator on both the server and the client.

This way, instead of the attribute implementing the C# logic and also accepting the
semantically equivalent JS expression, it would just be a generic custom attribute
accepting this DSL ruleset and deriving the server logic from it as well as marking
the form element on the client so that a geenralized client side logic could go through
these form elements and based on the same DSL ruleset, execute a JavaScript equivalent
of those checks.

Instead of using a custom attribute, then, a custom Fluent Validations chain could be
derived from the DSL instead and same on the client for the various client side
validation libraries.

https://fluentvalidation.net/start

https://github.com/JeremySkinner/FluentValidation#example

https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation

## To-Do

### Scaffold an ASP .NET MVC with Identity application

### Set up a form sending an example model to the application

### Make the model so that depending on a field another is either required or not

### Set up the required attribute on the server to use the validation context

Apply itself or not depending on the model values.

### Generate the client side validations based on the generalized declarative model

### Figure out how to distinguish form components marked by ASP .NET and custom attribute

See above.
