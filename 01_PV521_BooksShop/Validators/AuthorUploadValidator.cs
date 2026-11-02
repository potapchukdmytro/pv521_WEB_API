using FluentValidation;
using PV521_BooksShop.Controllers;

namespace PV521_BooksShop.Validators
{
    public class AuthorUploadValidator
        : AbstractValidator<AuthorUploadDto>
    {
        public AuthorUploadValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty()
                .WithMessage("Вкажіть ім'я автора")
                .MaximumLength(100)
                .WithMessage("Ім'я — максимум 100 символів");

            RuleFor(a => a.Biography)
                .MaximumLength(5000)
                .WithMessage("Біографія — максимум 5000 символів");

            RuleFor(a => a.Country)
                .MaximumLength(100)
                .WithMessage("Країна — максимум 100 символів");

            RuleFor(a => a.BirthDate)
                .NotNull()
                .WithMessage("Вкажіть дату народження")
                .Must(date =>
                    !date.HasValue ||
                    date.Value.Date <= DateTime.UtcNow.Date)
                .WithMessage(
                    "Дата народження не може бути в майбутньому");

            When(a => a.Photo != null, () =>
            {
                RuleFor(a => a.Photo!)
                    .Must(photo => photo.Length > 0)
                    .WithMessage("Файл порожній")
                    .Must(photo => photo.Length <= 5 * 1024 * 1024)
                    .WithMessage("Максимальний розмір фото — 5 МБ")
                    .Must(photo =>
                    {
                        string extension =
                            Path.GetExtension(photo.FileName)
                                .ToLowerInvariant();

                        return extension == ".jpg" ||
                               extension == ".jpeg" ||
                               extension == ".png";
                    })
                    .WithMessage("Дозволені тільки JPG та PNG");
            });
        }
    }
}
