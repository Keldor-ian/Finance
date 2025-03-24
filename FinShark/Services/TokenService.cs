using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace FinShark.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
        }
        public string CreateToken(AppUser user)
        {
            // Create claims that will be stored in the JWT token
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName)
            };

            // Sign the key and add secure hashing
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Add Token Descriptors (Information that is stored in the token)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"]
            };

            // Create and manage the token
            var tokenHandler = new JwtSecurityTokenHandler();

            // Takes the tokenDescriptor and generates the token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Converts the token to a JWT format and returns it to the user
            return tokenHandler.WriteToken(token);
        }
    }
}
