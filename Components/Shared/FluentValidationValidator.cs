using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Coursework.Components.Shared;

/// <summary>
/// Blazor-компонент, связывающий <see cref="EditContext"/> с FluentValidation-валидатором.
/// Регистрирует сообщения об ошибках в <see cref="ValidationMessageStore"/> при запросе
/// валидации или изменении отдельного поля.
/// </summary>
public class FluentValidationValidator : ComponentBase
{
    [CascadingParameter] private EditContext     EditContext      { get; set; } = default!;
    [Inject]             private IServiceProvider ServiceProvider { get; set; } = default!;

    protected override void OnInitialized()
    {
        var messages = new ValidationMessageStore(EditContext);

        EditContext.OnValidationRequested += (_, _) => Validate(messages);
        EditContext.OnFieldChanged        += (_, _) => Validate(messages);
    }

    private void Validate(ValidationMessageStore messages)
    {
        messages.Clear();

        var validatorType = typeof(IValidator<>).MakeGenericType(EditContext.Model.GetType());
        if (ServiceProvider.GetService(validatorType) is not IValidator validator) return;

        var context = new ValidationContext<object>(EditContext.Model);
        var result  = validator.Validate(context);

        foreach (var error in result.Errors)
            messages.Add(EditContext.Field(error.PropertyName), error.ErrorMessage);

        EditContext.NotifyValidationStateChanged();
    }
}
