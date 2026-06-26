using AutoMapper;
using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Doctor;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Exceptions;
using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Repositories;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HealthAxisCore_Api.Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;


        public DoctorService(
            IDoctorRepository repository,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<PagedResponseDTO<DoctorResponseDTO>> GetPagedAsync(
     int pageNumber,
     int pageSize,
     string? search,
     string? specialisation,
     string? status)
        {
            if (pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            var doctors = await _repository.GetAllAsync();

            var query = doctors.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    (!string.IsNullOrWhiteSpace(d.DoctorName) &&
                     d.DoctorName.Contains(search, StringComparison.OrdinalIgnoreCase)) ||

                    (!string.IsNullOrWhiteSpace(d.Email) &&
                     d.Email.Contains(search, StringComparison.OrdinalIgnoreCase)) ||

                    d.Specialisation.ToString().Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(specialisation) && specialisation != "All")
            {
                query = query.Where(d =>
                    d.Specialisation.ToString().Equals(
                        specialisation,
                        StringComparison.OrdinalIgnoreCase
                    ));
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                if (status.Equals("Active", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(d => d.IsActive);
                }
                else if (status.Equals("Inactive", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(d => !d.IsActive);
                }
            }

            var totalCount = query.Count();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedDoctors = query
                .OrderBy(d => d.DoctorName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var doctorDtos = _mapper.Map<List<DoctorResponseDTO>>(pagedDoctors);

            return new PagedResponseDTO<DoctorResponseDTO>
            {
                Items = doctorDtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }
        public async Task<IEnumerable<DoctorResponseDTO>> GetAllAsync()
        {
            var doctors = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorResponseDTO>>(doctors);
        }

        
        public async Task<DoctorResponseDTO?> GetByIdAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor not found");

            return _mapper.Map<DoctorResponseDTO>(doctor);
        }

        
        public async Task<DoctorResponseDTO> CreateAsync(CreateDoctorDTO dto)
        {
            
            var doctor = _mapper.Map<Doctor>(dto);
            doctor.CreatedDate = DateTime.Now;

            await _repository.AddAsync(doctor);

            
            var tempPassword = "Temp@" + new Random().Next(1000, 9999);

            
            var user = new ApplicationUser
            {
                UserName = doctor.Email,
                Email = doctor.Email,
                Role = "Doctor",
                ReferenceId = doctor.DoctorId,
                IsFirstLogin = true, 
                TemporaryPassword = tempPassword
            };

            var result = await _userManager.CreateAsync(user, tempPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BusinessRuleException(errors);
            }

            
            await _userManager.AddToRoleAsync(user, "Doctor");

            
            Console.WriteLine($"Doctor Temporary Password: {tempPassword}");

            

            return _mapper.Map<DoctorResponseDTO>(doctor);
        }

        
        public async Task<bool> UpdateAsync(int id, CreateDoctorDTO dto)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor not found");

            _mapper.Map(dto, doctor);

            await _repository.UpdateAsync(doctor);

            return true;
        }

        
        public async Task<bool> DeleteAsync(int id)
        {
            var exists = await _repository.Exists(id);

            if (!exists)
                throw new EntityNotFoundException("Doctor not found");

            await _repository.DeleteAsync(id);

            return true;
        }

        
        public async Task<IEnumerable<DoctorResponseDTO>> FilterAsync(
            string? name,
            SpecialisationType? specialization,
            bool? isActive)
        {
            var doctors = await _repository.GetDoctors(name, specialization, isActive);

            return _mapper.Map<IEnumerable<DoctorResponseDTO>>(doctors);
        }

        
        public async Task<bool> SetStatusAsync(int doctorId, bool status)
        {
            var doctor = await _repository.GetByIdAsync(doctorId);

            if (doctor == null)
                throw new EntityNotFoundException("Doctor not found");

            await _repository.SetStatus(doctorId, status);

            return true;
        }
    }
}
