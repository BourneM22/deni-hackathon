using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    interface IJwtService
    {
        String GenerateToken(String UserId);
    }
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly JwtConfig _jwtConfig;
        public JwtService(IConfiguration configuration, IOptions<JwtConfig> jwtConfig)
        {
            _configuration = configuration;
            _jwtConfig = jwtConfig.Value;
        }
        public String GenerateToken(String UserId)
        {
            DateTime tokenExpiredTime = DateTime.UtcNow.AddMinutes(_jwtConfig.TokenValidityMins);

            SecurityTokenDescriptor tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(JwtRegisteredClaimNames.Name, UserId)
                }),
                Expires = tokenExpiredTime,
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.Key)),
                    SecurityAlgorithms.HmacSha512Signature)
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDesc);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return accessToken;
        }
    }
}