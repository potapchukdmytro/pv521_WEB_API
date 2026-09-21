using FluentValidation;
using PV521_BooksShop.BLL.Dtos.Auth;
using PV521_BooksShop.DAL.Repositories;
using System.Text.RegularExpressions;

namespace PV521_BooksShop.BLL.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator(UserRepository userRepository)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Пошта є обов'язковою")
                .Matches(@"^[A-Za-z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?(?:\.[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?)+$", RegexOptions.IgnoreCase)
                .WithMessage("Невірний формат пошти")
                .MustAsync(async (email, ct) =>
                {
                    return !await userRepository.IsExistsEmailAsync(email, ct);
                }).WithMessage(value => $"Пошта '{value.Email}' вже використовується");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Ім'я користувача є обов'язковим")
                .MinimumLength(5).WithMessage("Мінімальна довжина 5 символів")
                .MustAsync(async (userName, ct) =>
                {
                    return !await userRepository.IsExistsUserNameAsync(userName, ct);
                }).WithMessage(value => $"Ім'я користувача '{value.UserName}' вже використовується");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль є обов'язковим")
                .MinimumLength(6).WithMessage("Мінімальна довжина 6 символів");


        }
    }
}
