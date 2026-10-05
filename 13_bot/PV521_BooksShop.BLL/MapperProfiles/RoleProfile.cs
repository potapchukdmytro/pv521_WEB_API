using AutoMapper;
using PV521_BooksShop.BLL.Dtos.Role;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.BLL.MapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            // Role -> RoleDto
            CreateMap<Role, RoleDto>();

            // CreateRoleDto -> Role
            CreateMap<CreateRoleDto, Role>();

            // UpdateRoleDto -> Role
            CreateMap<UpdateRoleDto, Role>();
        }
    }
}
