using FluentValidation;
using PV521_BooksShop.BLL.Dtos.Book;

namespace PV521_BooksShop.BLL.Validators.Book
{
    public class UpdateBookValidation : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id є обов'язковим")
                .GreaterThan(0).WithMessage("Id повинен бути більшим за 0");

            RuleFor(x => x.Language)
                .MaximumLength(50).WithMessage("Максимальна довжина 50 символів");

            RuleFor(x => x.Isbn)
                .MaximumLength(25).WithMessage("Максимальна довжина 25 символів");

            RuleFor(x => x.Publisher)
                .MaximumLength(100).WithMessage("Максимальна довжина 100 символів");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Назва книги є обов'язковою")
                .MaximumLength(255).WithMessage("Максимальна довжина 255 символів");

            RuleFor(x => x.Rating)
                .GreaterThanOrEqualTo(0).WithMessage("Рейтинг повинен бути від 0 до 10")
                .LessThanOrEqualTo(10).WithMessage("Рейтинг повинен бути від 0 до 10");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Ціна не може бути меншою за 0")
                .LessThanOrEqualTo(decimal.MaxValue).WithMessage($"Ціна не може бути більшою за {decimal.MaxValue}");

            RuleFor(x => x.Pages)
                .GreaterThan(0).WithMessage("К-сть сторінок не може бути меншою за 1")
                .LessThanOrEqualTo(int.MaxValue).WithMessage($"К-сть сторінок не може бути більшою за {int.MaxValue}");

            RuleFor(x => x.Year)
                .GreaterThan(0).WithMessage("Рік не може бути меншим за 1")
                .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage($"Рік не може бути більшим за {DateTime.UtcNow.Year}");
        }
    }
}
