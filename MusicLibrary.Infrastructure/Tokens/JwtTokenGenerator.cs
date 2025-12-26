using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MusicLibrary.Application.Auth.Interfaces;
using MusicLibrary.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MusicLibrary.Infrastructure.Tokens
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new InvalidOperationException("User email is missing.");

            var secret = _configuration["Jwt:Secret"]
                ?? throw new InvalidOperationException("Jwt:Secret is missing");

            var issuer = _configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("Jwt:Issuer is missing");

            var audience = _configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("Jwt:Audience is missing");

            var expiresMinutes = int.Parse(
                _configuration["Jwt:ExpiresMinutes"]
                ?? throw new InvalidOperationException("Jwt:ExpiresMinutes is missing")
            );

            var claims = new[]
           {
               new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
               new Claim(JwtRegisteredClaimNames.Email, user.Email),
           };

           var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
           var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

           var token = new JwtSecurityToken(
               issuer: issuer,
               audience: audience,
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
               signingCredentials: creds
           );

           return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
