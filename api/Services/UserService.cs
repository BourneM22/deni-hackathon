using api.DTO;
using api.Exceptions;
using api.Models;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface IUserService
    {
        Task<User> GetUserInfo(string userId);
        Task UpdateUserInfo(RegisterRequest registerRequest, String userId);
        Task UpdateProfilePicturePath(String newProfilePicturePath, String userId);
    }
    public class UserService : IUserService
    {
        private readonly DbConnection _dbConnection;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(DbConnection dbConnection, IPasswordHasher passwordHasher)
        {
            _dbConnection = dbConnection;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> GetUserInfo(string userId)
        {
            User? user = null;

            String query = "select name, gender, birth_date, email, password, profile_picture_path " +
                "from USER " + 
                "where user_id = ?;";

            using var conn = _dbConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                User tempUser = new User()
                {
                    Name = reader["NAME"].ToString()!,
                    Gender = Char.Parse(reader["GENDER"].ToString()!.Substring(0, 1)),
                    BirthDate = DateOnly.FromDateTime(DateTime.Parse(reader["BIRTH_DATE"].ToString()!)),
                    Email = reader["EMAIL"].ToString()!,
                    Password = reader["PASSWORD"].ToString()!,
                    ProfilePicturePath = reader["PROFILE_PICTURE_PATH"].ToString()!
                };

                user = tempUser;
            }

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            return user;
        }

        public async Task UpdateUserInfo(RegisterRequest registerRequest, String userId)
        {
            String query = "update USER " +
                "set name = ?, gender = ?, birth_date = ?, email = ?, password = ? " + 
                "where user_id = ?;";

            using var conn = _dbConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = registerRequest.Name });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = registerRequest.Gender });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = registerRequest.BirthDate });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = registerRequest.Email });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = _passwordHasher.Hash(registerRequest.Password) });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateProfilePicturePath(String newProfilePicturePath, String userId)
        {
            String query = "update USER " +
                "set profile_picture_path = ? " + 
                "where user_id = ?;";

            using var conn = _dbConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = newProfilePicturePath });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();
        }
    }
}