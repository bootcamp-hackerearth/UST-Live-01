using BCrypt.Net;
using HealthAxisHealth.Shared.DTOs.AuthDtos;
using HealthAxisHealth.Shared.Enums;
using HealthAxisHealth.API.Exceptions;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Models;
using HealthAxisHealth.API.Services.Interfaces;
using HealthAxisHealth.API.UnitOfWork;

namespace HealthAxisHealth.API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        #region Fields

        private readonly IUnitOfWork _unitOfWork;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        #endregion

        #region Constructor

        public AuthService(
            IUnitOfWork unitOfWork,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        #endregion

        #region Methods

        public async Task<RegisterResponseDto> RegisterAsync(
            RegisterDto dto)
        {
            User? existingUser =
                await _unitOfWork.Users
                    .GetByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                throw new BadRequestException(
                    "Email already exists.");
            }

            User user = new User
            {
                Email = dto.Email,
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = UserRole.Patient,
                IsActive = true
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            Patient patient = new Patient
            {
                UserId = user.UserId,
                FullName = dto.FullName,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email
            };

            await _unitOfWork.Patients.AddAsync(patient);
            await _unitOfWork.CommitAsync();

            string accessToken =
                _jwtTokenGenerator.GenerateToken(user);

            string refreshToken =
                Guid.NewGuid().ToString();

            user.SetRefreshToken(
                refreshToken,
                DateTime.UtcNow.AddDays(7));

            await _unitOfWork.Users.UpdateAsync(user);

            await _unitOfWork.CommitAsync();

            return new RegisterResponseDto
            {
                Status = "success",
                Message = "User registered successfully.",
                Data = new RegisterDataDto
                {
                    User = new RegisterUserDto
                    {
                        Id = user.UserId,
                        Email = user.Email,
                        FullName = patient.FullName,
                        CreatedAt = user.CreatedDate
                    },
                    Tokens = new RegisterTokenDto
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        ExpiresIn = 3600
                    }
                }
            };
        }

        public async Task<LoginResponseDto> LoginAsync(
            LoginDto dto)
        {
            User? user =
                await _unitOfWork.Users
                    .GetByEmailAsync(dto.Email);

            if (user == null)
            {
                throw new UnauthorizedException(
                    "Invalid credentials.");
            }

            bool isValidPassword =
                BCrypt.Net.BCrypt.Verify(
                    dto.Password,
                    user.PasswordHash);

            if (!isValidPassword)
            {
                throw new UnauthorizedException(
                    "Invalid credentials.");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedException(
                    "User account is inactive.");
            }

            string accessToken =
                _jwtTokenGenerator.GenerateToken(user);

            string refreshToken =
                Guid.NewGuid().ToString();

            user.SetRefreshToken(
                refreshToken,
                DateTime.UtcNow.AddDays(7));

            await _unitOfWork.Users.UpdateAsync(user);

            await _unitOfWork.CommitAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                TokenType = "Bearer",
                ExpiresIn = 3600,
                RefreshToken = refreshToken,
                User = new LoginUserDto
                {
                    Id = user.UserId,
                    Email = user.Email,
                    Roles = new List<string>
                    {
                        user.Role.ToString()
                    }
                }
            };
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(
            RefreshTokenDto dto)
        {
            User? user =
                await _unitOfWork.Users
                    .GetByRefreshTokenAsync(dto.RefreshToken);

            if (user == null ||
                !user.IsRefreshTokenValid())
            {
                throw new UnauthorizedException(
                    "Invalid refresh token.");
            }

            string accessToken =
                _jwtTokenGenerator.GenerateToken(user);

            string refreshToken =
                Guid.NewGuid().ToString();

            user.SetRefreshToken(
                refreshToken,
                DateTime.UtcNow.AddDays(7));

            await _unitOfWork.Users.UpdateAsync(user);

            await _unitOfWork.CommitAsync();

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                TokenType = "Bearer",
                ExpiresIn = 3600,
                RefreshToken = refreshToken,
                User = new LoginUserDto
                {
                    Id = user.UserId,
                    Email = user.Email,
                    Roles = new List<string>
                    {
                        user.Role.ToString()
                    }
                }
            };
        }

        public async Task LogoutAsync(
            string refreshToken)
        {
            User? user =
                await _unitOfWork.Users
                    .GetByRefreshTokenAsync(refreshToken);

            if (user == null)
            {
                throw new UnauthorizedException(
                    "Invalid refresh token.");
            }

            user.RevokeRefreshToken();

            await _unitOfWork.Users.UpdateAsync(user);

            await _unitOfWork.CommitAsync();
        }

        #endregion
    }
}
