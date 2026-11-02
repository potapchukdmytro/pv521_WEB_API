using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Repositories;
using PV521_BooksShop.Dtos;

namespace PV521_BooksShop.Services
{
    public class AuthorService
    {
        private readonly AuthorRepository _repository;

        public AuthorService(AuthorRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<AuthorDto>> GetAll()
        {
            return await _repository.Authors
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Biography = a.Biography,
                    Image = a.Image,
                    Country = a.Country,
                    BirthDate = a.BirthDate
                })
                .ToListAsync();
        }

        public async Task<AuthorDto?> GetById(int id)
        {
            var author = await _repository.GetByIdAsync(id);

            if (author == null)
            {
                return null;
            }

            return ToDto(author);
        }

        public async Task<AuthorDto> Create(CreateAuthorDto dto)
        {
            var author = new Author
            {
                Name = dto.Name,
                Biography = dto.Biography,
                Image = dto.Image,
                Country = dto.Country,
                BirthDate = GetBirthDate(dto)
            };

            await _repository.CreateAsync(author);

            return ToDto(author);
        }

        public async Task<AuthorDto?> Update(
            int id,
            UpdateAuthorDto dto)
        {
            var author = await _repository.GetByIdAsync(id);

            if (author == null)
            {
                return null;
            }

            author.Name = dto.Name;
            author.Biography = dto.Biography;
            author.Image = dto.Image;
            author.Country = dto.Country;
            author.BirthDate = GetBirthDate(dto);

            await _repository.UpdateAsync(author);

            return ToDto(author);
        }

        public async Task<AuthorDto?> Delete(int id)
        {
            var author = await _repository.GetByIdAsync(id);

            if (author == null)
            {
                return null;
            }

            var dto = ToDto(author);

            bool deleted = await _repository.DeleteAsync(author);

            return deleted ? dto : null;
        }

        private static DateTime GetBirthDate(CreateAuthorDto dto)
        {
            if (dto.BirthDate == null)
            {
                throw new ArgumentException(
                    "Вкажіть дату народження");
            }

            return dto.BirthDate.Value.ToUniversalTime();
        }

        private static AuthorDto ToDto(Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Biography = author.Biography,
                Image = author.Image,
                Country = author.Country,
                BirthDate = author.BirthDate
            };
        }
    }
}
