using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.BLL.Dtos.Book;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.BLL.Validators.Book
{
    public class CreateBookValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookValidator(AuthorRepository authorRepository)
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Назва книги є обов'язковою")
                .MaximumLength(255).WithMessage("Максимальна довжина 255 символів");

            RuleFor(x => x.Language)
                .MaximumLength(50).WithMessage("Максимальна довжина 50 символів");

            RuleFor(x => x.Isbn)
                .MaximumLength(25).WithMessage("Максимальна довжина 25 символів");

            RuleFor(x => x.Publisher)
                .MaximumLength(100).WithMessage("Максимальна довжина 100 символів");

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

            //RuleFor(x => x.AuthorId)
            //    .MustAsync(async (authorId, cancellationToken) =>
            //    {
            //        return await authorRepository.Authors.AnyAsync(a => a.Id == authorId, cancellationToken);
            //    }).WithMessage(x => $"Автор з id '{x.AuthorId}' не існує");
        }
    }
}
