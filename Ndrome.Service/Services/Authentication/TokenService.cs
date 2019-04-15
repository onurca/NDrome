using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Ndrome.Service.Services
{
    public class TokenService
    {
        private readonly string JwtKey;
        private readonly string JwtIssuer;

        public TokenService()
        {

            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            JwtKey = root.GetSection("Jwt").GetSection("Key").Value;
            JwtIssuer = root.GetSection("Jwt").GetSection("Issuer").Value;
        }

        public string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name,username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: JwtIssuer,
                audience: JwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public string Authenticate(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var validator = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters();
            validationParameters.ValidIssuer = JwtIssuer;
            validationParameters.ValidAudience = JwtIssuer;
            validationParameters.IssuerSigningKey = key;
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.ValidateAudience = true;

            if (!validator.CanReadToken(token))
                return null;

            try
            {
                var principal = validator.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if (principal.HasClaim(c => c.Type == ClaimTypes.Name))
                {
                    return principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }
    }
}
