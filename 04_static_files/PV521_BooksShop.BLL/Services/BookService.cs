using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.BLL.Dtos;
using PV521_BooksShop.BLL.Dtos.Book;
using PV521_BooksShop.BLL.Dtos.Pagination;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.BLL.Services
{
    public class BookService
    {
        private readonly BookRepostiory _bookRepostiory;
        private readonly ImageService _imageService;
        private readonly IMapper _mapper;

        public BookService(BookRepostiory bookRepostiory, IMapper mapper, ImageService imageService)
        {
            _bookRepostiory = bookRepostiory;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<ServiceResponseDto> GetAllAsync(PaginationRequestDto dto)
        {
            int total = await _bookRepostiory.Books.CountAsync();
            dto.PageSize = dto.PageSize < 1 ? 50 : dto.PageSize;

            int pages = (int)Math.Ceiling((double)total / dto.PageSize);
            dto.Page = dto.Page < 1 || dto.Page > pages ? 1 : dto.Page;

            var entities = await _bookRepostiory.Books
                .Include(b => b.Author)
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<BookDto>>(entities);

            var paginationResponse = new PaginationResponseDto<BookDto>
            {
                Items = dtos,
                Page = dto.Page,
                PageSize = dto.PageSize,
                Total = total,
                PageCount = pages
            };

            return ServiceResponseDto.Success("Книги отримано", paginationResponse);
        }

        public async Task<ServiceResponseDto> GetByIdAsync(int id)
        {
            var entity = await _bookRepostiory.GetByIdAsync(id, true);

            if (entity == null)
            {
                return ServiceResponseDto.Error($"Книгу з id '{id}' не знайдено");
            }

            var dto = _mapper.Map<BookDto>(entity);

            return ServiceResponseDto.Success("Книгу отримано", dto);
        }

        public async Task<ServiceResponseDto> DeleteAsync(int id)
        {
            bool res = await _bookRepostiory.DeleteAsync(id);

            if (res)
            {
                return ServiceResponseDto.Success("Книгу видалено");
            }

            return ServiceResponseDto.Success("Не вдалося видалити книгу");
        }

        public async Task<ServiceResponseDto> CreateAsync(CreateBookDto dto, string imagesPath)
        {
            if (string.IsNullOrEmpty(dto.Title))
            {
                return ServiceResponseDto.Error("Назва книги є обов'язковою");
            }

            var entity = _mapper.Map<Book>(dto);

            if(dto.Image != null)
            {
                // Save image
                entity.Image = await _imageService.SaveAsync(dto.Image, imagesPath);
            }

            bool res = await _bookRepostiory.CreateAsync(entity);

            if (!res)
            {
                return ServiceResponseDto.Success("Не вдалося додати книгу");
            }

            return ServiceResponseDto.Success("Книгу додано", _mapper.Map<BookDto>(entity));
        }

        public async Task<ServiceResponseDto> UpdateAsync(UpdateBookDto dto)
        {
            if (string.IsNullOrEmpty(dto.Title))
            {
                return ServiceResponseDto.Error("Назва книги є обов'язковою");
            }

            var entity = await _bookRepostiory.GetByIdAsync(dto.Id);

            if (entity == null)
            {
                return ServiceResponseDto.Error($"Книгу з id '{dto.Id}' не знайдено");
            }

            _mapper.Map(dto, entity);

            bool res = await _bookRepostiory.UpdateAsync(entity);

            if (!res)
            {
                return ServiceResponseDto.Success("Не вдалося оновити книгу");
            }

            return ServiceResponseDto.Success("Книгу оновлено", _mapper.Map<BookDto>(entity));
        }
    }
}
