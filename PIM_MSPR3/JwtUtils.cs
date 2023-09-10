using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PIM_MSPR3.Model;

namespace StackTim_TP
{
    public class JwtUtils
    {
        private readonly IConfiguration? _configuration;

        public JwtUtils(IConfiguration? configuration)
        {
            _configuration = configuration;
        }
        public static Dictionary<string, string> DecodeJwt(string tokenString, string secret)
        {
            var handler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            SecurityToken validatedToken;
            var claims = handler.ValidateToken(tokenString, validationParameters, out validatedToken).Claims;
            var result = new Dictionary<string, string>();
            foreach (var claim in claims)
            {
                result[claim.Type] = claim.Value;
            }

            return result;
        }

        public string CreateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.CodeUser.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.MailUser),
            }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

    }
}