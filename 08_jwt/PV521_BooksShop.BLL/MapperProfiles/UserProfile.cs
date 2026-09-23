using AutoMapper;
using PV521_BooksShop.BLL.Dtos.User;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.BLL.MapperProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // User -> UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : "User"));
        }
    }
}
