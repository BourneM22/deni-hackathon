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
        Task SetNewPassword(String password, String email);
        Task ResetPassword(String email);
    }

    public class AuthService : IAuthService
    {
        private readonly DbConnection _dbConnection;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtService _jwtService;
        private readonly JwtConfig _jwtConfig;
        private readonly IEmailService _emailService;

        public AuthService(DbConnection dbConnection, IPasswordHasher passwordHasher, IJwtService jwtService, IOptions<JwtConfig> jwtConfig, IEmailService emailService)
        {
            _dbConnection = dbConnection;
            _passwordHasher = passwordHasher;
            _jwtService = jwtService;
            _jwtConfig = jwtConfig.Value;
            _emailService = emailService;
        }

        public async Task<LoginResponse> Authenticate(LoginRequest request)
        {
            String hashedPassword = String.Empty;
            String userId = String.Empty;

            String query = "select user_id, password " +
                "from USER " +
                "where email = ?;";

            using MySqlConnection conn1 = _dbConnection.GetConnection();
            using MySqlCommand cmd1 = new MySqlCommand(query, conn1);

            cmd1.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = request.Email });

            using var reader = await cmd1.ExecuteReaderAsync();

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

            query = "update USER " +
                "set last_login = ? " +
                "where user_id = ?;";

            using MySqlConnection conn2 = _dbConnection.GetConnection();
            using MySqlCommand cmd2 = new MySqlCommand(query, conn2);

            cmd2.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = DateTime.Now });
            cmd2.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd2.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
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

            String query = "insert into USER (user_id, email, birth_date, created_date_time, gender, name, password, is_admin) " +
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
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = newUser.IsAdmin });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
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

        public async Task SetNewPassword(String password, String email)
        {
            String query = "update USER " +
                "set password = ? " + 
                "where email = ?;";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = _passwordHasher.Hash(password) });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = email });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task ResetPassword(string email)
        {
            String newPassword = GenerateRandomString(10);

            await SetNewPassword(newPassword, email);
            await _emailService.SendEmail(email, "Your Deni app password has been reset!", $"Your new Deni password: {newPassword}");
        }

        public string GenerateRandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}