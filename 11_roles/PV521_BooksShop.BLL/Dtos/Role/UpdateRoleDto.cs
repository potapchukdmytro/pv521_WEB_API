using FluentValidation;
using PV521_BooksShop.BLL.Dtos.Role;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.BLL.Validators.Role
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleDto>
    {
        public CreateRoleValidator(RoleRepository repository)
        {
            RuleFor(dto => dto.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Вкажіть назву ролі")
                .MaximumLength(50).WithMessage("Назва має містити до 50 символів")
                .MustAsync(async (name, ct) =>
                    !await repository.IsExistsAsync(name, ct))
                .WithMessage("Роль із такою назвою вже існує");
        }
    }
}
