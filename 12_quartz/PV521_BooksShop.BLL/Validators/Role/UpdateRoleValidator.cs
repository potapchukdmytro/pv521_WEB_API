using FluentValidation;
using PV521_BooksShop.BLL.Dtos.Role;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.BLL.Validators.Role
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleValidator(RoleRepository roleRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id повинна бути більшою за 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва ролі є обов'язковою")
                .MustAsync(async (name, ct) => !await roleRepository.IsExistsAsync(name, ct))
                .WithMessage(x => $"Роль з іменем '{x.Name}' вже існує");
        }
    }
}
