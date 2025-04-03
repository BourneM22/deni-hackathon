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
    public interface IJwtService
    {
        String GenerateToken(String userId);
        String GetUserIdFromToken(string token);
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
        public String GenerateToken(String userId)
        {
            DateTime tokenExpiredTime = DateTime.UtcNow.AddMinutes(_jwtConfig.TokenValidityMins);

            SecurityTokenDescriptor tokenDesc = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new []
                {
                    new Claim(JwtRegisteredClaimNames.Name, userId)
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

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        public string? GetUserIdFromToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();

            try
            {
                // Decode the token without validating its signature (just parsing the token)
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken == null)
                    return null;  // Invalid token format

                // Validate the token's signature and claims here, if necessary
                // You can validate the token using the Issuer, Audience, and the SigningKey

                // Retrieve the user ID from the "Name" claim (which you set in your GenerateToken function)
                var userId = jsonToken?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Name)?.Value;

                return userId;  // Return the user ID or null if not found
            }
            catch (Exception)
            {
                return null;  // Token is invalid or parsing failed
            }
        }
    }
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
}