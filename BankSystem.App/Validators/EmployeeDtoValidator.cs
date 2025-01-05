using BankSystem.App.DTO;
using BankSystem.Models;
using FluentValidation;

namespace BankSystem.App.Validators;

public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
{
    public EmployeeDtoValidator()
    {
        RuleFor(e => e.FullName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Имя пользователя обязательно.");

        RuleFor(e => e.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .WithMessage("Паспорт обязателен");

        RuleFor(e => e.Age)
            .NotNull()
            .NotEmpty()
            .WithMessage("Введите возраст")
            .GreaterThan(17)
            .WithMessage("Вам меньше 18 лет");

        RuleFor(e => e.PhoneNumber)
            .NotNull()
            .NotEmpty()
            .MaximumLength(15)
            .WithMessage("Введите корректный комер телефона");
    }
}