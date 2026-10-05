using AutoMapper;
using PV521_BooksShop.BLL.Dtos.Book;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.BLL.MapperProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            // Book -> BookDto
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image ?? "bookDefault.png"));

            // CreateBookDto -> Book
            CreateMap<CreateBookDto, Book>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());

            // UpdateBookDto -> Book
            CreateMap<UpdateBookDto, Book>()
                .ForMember(dest => dest.Image, opt => opt.Ignore());
        }
    }
}
