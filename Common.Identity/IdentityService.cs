using Common.Identity.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common.Identity
{
    public class IdentityService: IIdentityService
    {
        public readonly JwtSettings _jwtSettings;
        private readonly IConfiguration _configuration;
        public IdentityService(IConfiguration configuration)
        {
            _configuration = configuration;
            _jwtSettings = new JwtSettings
            {
                Key = _configuration["JwtSettings:Key"] ?? string.Empty,
                Issuer = _configuration["JwtSettings:Issuer"] ?? string.Empty,
                Audience = _configuration["JwtSettings:Audience"] ?? string.Empty,
                DurationInMinutes = int.TryParse(_configuration["JwtSettings:DurationInMinutes"], out var duration) ? duration : 10
            };
        }
        public async Task<string> GenerateJwtToken(LoginModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.MobileNo),
                //new Claim(ClaimTypes.Role, user.Role),
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
