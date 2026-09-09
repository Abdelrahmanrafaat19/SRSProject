using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SRSProject.Application.Contracts;
using SRSProject.Infrastructure.DataContext.IDentityEntity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SRSProject.Infrastructure.Jwt
{
    public class JwtCreator : IJwtCreator
    {
        private readonly UserManager<UserEntity> _user;
        private readonly IOptions<JwtSettings> _jwtSettings;
        public JwtCreator(UserManager<UserEntity> user, IOptions<JwtSettings> jwtSettings)
        {
            _user = user;
            _jwtSettings = jwtSettings;
        }
        public string CreateToken(string? email, string userName, string id, IList<string>? Roles, CancellationToken cancellationToken)
        {
            #region Claims
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email ?? string.Empty),
                new Claim(ClaimTypes.Name,userName!),
                new Claim(ClaimTypes.NameIdentifier,id!),
            };
            Claims.AddRange(Roles!.Select(role => new Claim(ClaimTypes.Role, role)));
            #endregion
            #region Credentials Certificate
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.Key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            #endregion


            var token = new JwtSecurityToken
                (
                        issuer: _jwtSettings.Value.Issuer,
                        audience: _jwtSettings.Value.Audience,
                        claims: Claims,
                        expires: DateTime.UtcNow.AddMinutes(_jwtSettings.Value.DurationInMinutes),
                        signingCredentials: credentials
                  );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


    public class JwtSettings
    {
        public string Key { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public int DurationInMinutes { get; set; }
    }
}
