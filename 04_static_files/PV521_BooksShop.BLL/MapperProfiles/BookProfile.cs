using AutoMapper;
using PV521_BooksShop.BLL.Dtos.Author;
using PV521_BooksShop.BLL.Dtos.Book;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.BLL.MapperProfiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            // Book -> BookDto
            CreateMap<Book, BookDto>();

            // CreateBookDto -> Book
            CreateMap<CreateBookDto, Book>();

            // UpdateBookDto -> Book
            CreateMap<UpdateBookDto, Book>();
        }
    }
}
