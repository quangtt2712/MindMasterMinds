using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MindMasterMinds_Data;
using MindMasterMinds_Data.Entities;
using MindMasterMinds_Data.Models.Internal;
using MindMasterMinds_Data.Models.Requests.Post;
using MindMasterMinds_Data.Models.Views;
using MindMasterMinds_Service.Interfaces;
using MindMasterMinds_Utility.Exceptions;
using MindMasterMinds_Utility.Helpers;
using MindMasterMinds_Utility.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MindMasterMinds_Service.Implementations
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AppSetting _appSettings;
        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSetting> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<AuthModel> GetAuth(Guid id)
        {
            var auth = await _unitOfWork.User.GetMany(account => account.Id.Equals(id))
                                                .Include(account => account.UserRole)
                                                .FirstOrDefaultAsync();
            if (auth != null)
            {
                return new AuthModel
                {
                    Id = auth.Id,
                    Role = auth.UserRole!.RoleName,
                    IsDeleted = auth.IsDeleted
                };
            }
            throw new NotFoundException("Không tìm thấy account.");
        }
        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        private string GenerateJwtToken(AuthModel auth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", auth.Id.ToString()),

                    new Claim("role", auth.Role.ToString()),

                    new Claim("isDeleted", auth.IsDeleted.ToString()),
                }),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<TokenViewModel> Authenticated(AuthRequest auth)
        {
            var user = await _unitOfWork.User.GetMany(user => user.Email == auth.Email)
                                                .Include(user => user.UserRole)
                                                .FirstOrDefaultAsync();

            if (user != null && PasswordHasher.VerifyPassword(auth.Password + user.PasswordSalt, user.PasswordHash!))
            {
                if (user.IsDeleted || !user.EmailConfirmed)
                {
                    throw new BadRequestException("Tài khoản của bạn đã bị khóa hoặc chưa xác thực email");
                }
                var accessToken = GenerateJwtToken(new AuthModel
                {
                    Id = user.Id,
                    Role = user.UserRole!.RoleName,
                    IsDeleted = user.IsDeleted
                });

                var refreshToken = GenerateRefreshToken();

                var refreshTokenOfUser = await _unitOfWork.RefreshToken.GetMany(token => token.UserId == user.Id).ToListAsync();
                if (refreshTokenOfUser != null || refreshTokenOfUser!.Any())
                {
                    _unitOfWork.RefreshToken.RemoveRange(refreshTokenOfUser!);
                    await _unitOfWork.SaveChangesAsync();
                }

                var newRefreshToken = new RefreshToken
                {
                    UserId = user.Id,
                    Token = refreshToken,
                    ExpirationDate = DateTime.UtcNow.AddDays(1),
                    CreationDate = DateTime.UtcNow,
                    IsUsed = false
                };

                _unitOfWork.RefreshToken.Add(newRefreshToken);
                await _unitOfWork.SaveChangesAsync();

                return new TokenViewModel
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    UserViewLogin = _mapper.Map<UserViewModel>(user)
                };
            }
            throw new NotFoundException("Sai tài khoản hoặc mật khẩu.");
        }

    }
}
