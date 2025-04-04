using api.DTO;
using api.Enum;
using api.Exceptions;
using api.Models;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> Authenticate(LoginRequest request);
        Task Register(RegisterRequest registerRequest);
        Task<bool> CheckEmailAlreadyExist(String email);
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

        public async Task<LoginResponse> Authenticate(LoginRequest request)
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

            if (!_passwordHasher.Verify(hashedPassword, request.Password))
            {
                throw new PasswordNotMatchException();
            }

            return new LoginResponse()
            {
                TokenType = "Bearer",
                AccessToken = _jwtService.GenerateToken(userId),
                ExpiresInMinutes = _jwtConfig.TokenValidityMins
            };
        }

        public async Task Register(RegisterRequest registerRequest)
        {
            if (await CheckEmailAlreadyExist(registerRequest.Email))
            {
                throw new EmailAlreadyExistException();
            }

            User newUser = new User()
            {
                UserId = Guid.NewGuid().ToString(),
                CreatedDateTime = DateTime.Now,
                Email = registerRequest.Email,
                BirthDate = registerRequest.BirthDate,
                Gender = registerRequest.Gender,
                Name = registerRequest.Name,
                Password = _passwordHasher.Hash(registerRequest.Password),
                IsAdmin = IsAdmin.USER
            };

            String query = "insert into USER (user_id, email, birth_date, created_date_time, gender, name, password, admin) " +
                "values (?, ?, ?, ?, ?, ?, ?, ?);";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = newUser.UserId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = newUser.Email });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = newUser.BirthDate });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = newUser.CreatedDateTime });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = newUser.Gender });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = newUser.Name });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = newUser.Password });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = newUser.IsAdmin });

            int res = await cmd.ExecuteNonQueryAsync();
        }

        public async Task<bool> CheckEmailAlreadyExist(String email)
        {
            String query = "select email " +
                "from USER " + 
                "where email = ?;";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = email });

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return true;
            }

            return false;
        }
    }
}