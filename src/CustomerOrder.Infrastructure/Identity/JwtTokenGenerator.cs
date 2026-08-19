using CustomerOrder.Application.Dtos.Auth;
using CustomerOrder.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace CustomerOrder.Infrastructure.Identity
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _key;
        private readonly int _expiryMinutes;

        public JwtTokenGenerator()
        {
            _issuer = ConfigurationManager.AppSettings["Jwt:Issuer"];
            _audience = ConfigurationManager.AppSettings["Jwt:Audience"];
            _key = ConfigurationManager.AppSettings["Jwt:Key"];
            _expiryMinutes = int.Parse(ConfigurationManager.AppSettings["Jwt:ExpiryMinutes"]);
        }

        public TokenDto Generate(AuthenticatedUser user)
        {
            var expiresAtUtc = DateTime.UtcNow.AddMinutes(_expiryMinutes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expiresAtUtc,
                signingCredentials: credentials);

            return new TokenDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                TokenType = "Bearer",
                ExpiresAtUtc = expiresAtUtc,
                UserName = user.UserName
            };
        }
    }
}
