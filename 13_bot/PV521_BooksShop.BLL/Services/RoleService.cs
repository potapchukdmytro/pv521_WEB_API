using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.BLL.Dtos;
using PV521_BooksShop.BLL.Dtos.Role;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.BLL.Services
{
    public class RoleService
    {
        private readonly RoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(RoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponseDto> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _roleRepository.Roles.ToListAsync(ct);

            var dtos = _mapper.Map<List<RoleDto>>(entities);

            return ServiceResponseDto.Success("Ролі отримано", dtos);
        }

        public async Task<ServiceResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _roleRepository.GetByIdAsync(id, ct);

            if(entity == null)
            {
                return ServiceResponseDto.Error($"Роль з id '{id}' не знайдена");
            }

            var dto = _mapper.Map<RoleDto>(entity);

            return ServiceResponseDto.Success("Роль отримано", dto);
        }

        public async Task<ServiceResponseDto> GetByNameAsync(string name, CancellationToken ct = default)
        {
            var entity = await _roleRepository.GetByNameAsync(name, ct);

            if (entity == null)
            {
                return ServiceResponseDto.Error($"Роль '{name}' не знайдена");
            }

            var dto = _mapper.Map<RoleDto>(entity);

            return ServiceResponseDto.Success("Роль отримано", dto);
        }

        public async Task<ServiceResponseDto> DeleteAsync(int id, CancellationToken ct = default)
        {
            bool res = await _roleRepository.DeleteAsync(id);

            if(!res)
            {
                return ServiceResponseDto.Error("Помилка під час видалення ролі");
            }

            return ServiceResponseDto.Success("Роль успішно видалено");
        }

        public async Task<ServiceResponseDto> CreateAsync(CreateRoleDto dto, CancellationToken ct = default)
        {
            var entity = _mapper.Map<Role>(dto);

            bool res = await _roleRepository.CreateAsync(entity, ct);

            if (!res)
            {
                return ServiceResponseDto.Error("Помилка під час додавання ролі");
            }

            return ServiceResponseDto.Success("Роль успішно додано", _mapper.Map<RoleDto>(entity));
        }

        public async Task<ServiceResponseDto> UpdateAsync(UpdateRoleDto dto, CancellationToken ct = default)
        {
            var entity = await _roleRepository.GetByIdAsync(dto.Id, ct);

            if (entity == null)
            {
                return ServiceResponseDto.Error($"Роль з id '{dto.Id}' не знайдена");
            }

            _mapper.Map(dto, entity);

            bool res = await _roleRepository.UpdateAsync(entity, ct);

            if (!res)
            {
                return ServiceResponseDto.Error("Помилка під час змінення ролі");
            }

            return ServiceResponseDto.Success("Роль успішно змінено", dto);
        }
    }
}
