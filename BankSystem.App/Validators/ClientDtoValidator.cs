using BankSystem.App.DTO;
using FluentValidation;

namespace BankSystem.App.Validators;

public class ClientDtoValidator : AbstractValidator<ClientDto>
{
    public ClientDtoValidator()
    {
        RuleFor(c => c.FullName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Имя пользователя обязательно.");

        RuleFor(c => c.PasNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage("Паспорт обязателен");

        RuleFor(c => c.Age)
            .NotNull()
            .NotEmpty()
            .WithMessage("Введите возраст")
            .GreaterThan(17)
            .WithMessage("Вам меньше 18 лет");

        RuleFor(c => c.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .MaximumLength(15)
            .WithMessage("Введите корректный комер телефона");
    }
}