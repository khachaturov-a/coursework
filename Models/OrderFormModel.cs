using FluentValidation;

namespace Coursework.Models;

/// <summary>Данные покупателя, вводимые при оформлении заказа.</summary>
public class OrderFormModel
{
    /// <summary>Имя покупателя.</summary>
    public string  CustomerName  { get; set; } = string.Empty;

    /// <summary>Контактный телефон покупателя.</summary>
    public string  CustomerPhone { get; set; } = string.Empty;

    /// <summary>Необязательный комментарий к заказу.</summary>
    public string? Comment       { get; set; }
}

/// <summary>Правила валидации формы оформления заказа (FluentValidation).</summary>
public class OrderFormValidator : AbstractValidator<OrderFormModel>
{
    public OrderFormValidator()
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Введите ваше имя")
            .MinimumLength(2).WithMessage("Имя должно содержать не менее 2 символов")
            .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов");

        RuleFor(x => x.CustomerPhone)
            .NotEmpty().WithMessage("Введите номер телефона")
            .Matches(@"^[\+]?[\d\s\-\(\)]{7,20}$")
            .WithMessage("Введите корректный номер телефона (от 7 до 20 цифр)");
    }
}
