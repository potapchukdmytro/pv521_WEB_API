using AutoMapper;
using PV521_BooksShop.BLL.Dtos.Author;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.BLL.MapperProfiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            // Author -> AuthorDto
            CreateMap<Author, AuthorDto>();
        }
    }
}
