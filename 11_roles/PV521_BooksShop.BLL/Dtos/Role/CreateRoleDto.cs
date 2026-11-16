using FluentValidation;
using PV521_BooksShop.BLL.Dtos.Role;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.BLL.Validators.Role
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleValidator(RoleRepository repository)
        {
            RuleFor(dto => dto.Id)
                .GreaterThan(0).WithMessage("ID ролі має бути більшим за 0");

            RuleFor(dto => dto.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Вкажіть назву ролі")
                .MaximumLength(50).WithMessage("Назва має містити до 50 символів")
                .MustAsync(async (dto, name, ct) =>
                {
                    var existing = await repository.GetByNameAsync(name, ct);
                    return existing == null || existing.Id == dto.Id;
                })
                .WithMessage("Роль із такою назвою вже існує");
        }
    }
}
