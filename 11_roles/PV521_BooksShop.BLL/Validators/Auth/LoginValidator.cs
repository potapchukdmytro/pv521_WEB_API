using FluentValidation;
using PV521_BooksShop.BLL.Dtos.Auth;

namespace PV521_BooksShop.BLL.Validators.Auth
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(dto => dto.CurrentPassword)
                .NotEmpty()
                .WithMessage("Вкажіть поточний пароль");

            RuleFor(dto => dto.NewPassword)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Вкажіть новий пароль")
                .MinimumLength(6)
                .WithMessage("Мінімальна довжина пароля — 6 символів")
                .NotEqual(dto => dto.CurrentPassword)
                .WithMessage("Новий пароль має відрізнятися від поточного");
        }
    }
}
