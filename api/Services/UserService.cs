using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();
    }
    public class UserService : IUserService
    {
        private readonly DbConnection _dbConnection;
        public UserService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = new List<User>();

            String query = "select user_id, name, gender, birth_date, email, password, profile_picture_path, created_date_time " +
                "from USER;";

            using var conn = _dbConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                User user = new User()
                {
                    UserId = reader["USER_ID"].ToString()!,
                    Name = reader["NAME"].ToString()!,
                    Gender = Char.Parse(reader["GENDER"].ToString()!.Substring(0, 1)),
                    BirthDate = DateOnly.FromDateTime(DateTime.Parse(reader["BIRTH_DATE"].ToString()!)),
                    Email = reader["EMAIL"].ToString()!,
                    Password = reader["PASSWORD"].ToString()!,
                    ProfilePicturePath = reader["PROFILE_PICTURE_PATH"].ToString()!,
                    CreatedDateTime = DateTime.Parse(reader["CREATED_DATE_TIME"].ToString()!),
                };

                users.Add(user);
            }

            return users;
        }
    }
}