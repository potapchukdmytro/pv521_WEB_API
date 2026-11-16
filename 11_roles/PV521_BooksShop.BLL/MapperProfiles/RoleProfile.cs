using AutoMapper;
using PV521_BooksShop.BLL.Dtos.Role;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.BLL.MapperProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>();

            CreateMap<CreateRoleDto, Role>()
                .ForMember(role => role.Name,
                    options => options.MapFrom(dto => dto.Name.Trim()));

            CreateMap<UpdateRoleDto, Role>()
                .ForMember(role => role.Id, options => options.Ignore())
                .ForMember(role => role.Name,
                    options => options.MapFrom(dto => dto.Name.Trim()));
        }
    }
}
