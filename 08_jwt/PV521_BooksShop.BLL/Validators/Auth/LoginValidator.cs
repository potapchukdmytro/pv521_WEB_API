using FluentValidation;
using PV521_BooksShop.BLL.Dtos.Auth;
using System.Text.RegularExpressions;

namespace PV521_BooksShop.BLL.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Пошта є обов'язковою")
                .Matches(@"^[A-Za-z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?(?:\.[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?)+$", RegexOptions.IgnoreCase)
                .WithMessage("Невірний формат пошти");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль є обов'язковим")
                .MinimumLength(6).WithMessage("Мінімальна довжина 6 символів");
        }
    }
}
