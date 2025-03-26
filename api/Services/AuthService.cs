using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using api.DTO;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> authenticate(LoginRequest request);
    }

    public class AuthService : IAuthService
    {
        private readonly DbConnection _dbConnection;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly JwtConfig _jwtConfig;

        public AuthService(DbConnection dbConnection, IPasswordHasher passwordHasher, IJwtService jwtService, IOptions<JwtConfig> jwtConfig)
        {
            _dbConnection = dbConnection;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _jwtConfig = jwtConfig.Value;
        }

        public async Task<LoginResponse> authenticate(LoginRequest request)
        {
            String hashedPassword = String.Empty;
            String userId = String.Empty;

            String query = "select user_id, password " +
                "from USER " +
                "where email = ?;";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = request.Email });

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                hashedPassword = reader["PASSWORD"].ToString()!;
                userId = reader["USER_ID"].ToString()!;
            }

            if (String.IsNullOrEmpty(hashedPassword) || String.IsNullOrEmpty(userId))
            {
                throw new EmailNotFoundException();
            }

            if (!_passwordHasher.verify(hashedPassword, request.Password))
            {
                throw new PasswordNotMatchException();
            }

            return new LoginResponse()
            {
                TokenType = "Bearer",
                AccessToken = _jwtService.generateToken(userId),
                ExpiresIn = _jwtConfig.TokenValidityMins
            };
        }
    }
}