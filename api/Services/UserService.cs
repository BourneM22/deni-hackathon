using api.DTO;
using api.Enum;
using api.Exceptions;
using api.Models;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface IUserService
    {
        Task<UserInfoResponse> GetUserInfo(string userId);
        Task UpdateUserInfo(RegisterRequest registerRequest, String userId);
        Task UpdateProfilePicturePath(String newProfilePicturePath, String userId);
        Task<ProfilePictureBytes> GetProfilePictureByte(String userId);
        Task<Boolean> CheckIsAdmin(String userId);
        Task DeleteProfilePicture(String userId);
    }
    public class UserService : IUserService
    {
        private readonly DbConnection _dbConnection;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileService _fileService;

        public UserService(DbConnection dbConnection, IPasswordHasher passwordHasher, IFileService fileService)
        {
            _dbConnection = dbConnection;
            _passwordHasher = passwordHasher;
            _fileService = fileService;
        }

        public async Task<UserInfoResponse> GetUserInfo(string userId)
        {
            UserInfoResponse? user = null;

            String query = "select name, gender, birth_date, email, password, profile_picture_path " +
                "from USER " + 
                "where user_id = ?;";

            using var conn = _dbConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                UserInfoResponse tempUser = new UserInfoResponse()
                {
                    Name = reader["NAME"].ToString()!,
                    Gender = Char.Parse(reader["GENDER"].ToString()!.Substring(0, 1)),
                    BirthDate = DateOnly.FromDateTime(DateTime.Parse(reader["BIRTH_DATE"].ToString()!)),
                    Email = reader["EMAIL"].ToString()!
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

            if (res == 0)
            {
                throw new Exception();
            }
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

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task<ProfilePictureBytes> GetProfilePictureByte(String userId)
        {
            String profilePicturePath = String.Empty;

            String query = "select profile_picture_path " +
                "from USER " + 
                "where user_id = ?;";

            using var conn = _dbConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                if (reader["PROFILE_PICTURE_PATH"] != DBNull.Value)
                {
                    profilePicturePath = reader["PROFILE_PICTURE_PATH"].ToString()!;
                }
            }

            if (String.IsNullOrEmpty(profilePicturePath))
            {
                String imgName = "default.jpg";
                return new ProfilePictureBytes()
                {
                    ImgBytes = _fileService.GetImageByte(imgName),
                    FileName = imgName
                };
            }

            return new ProfilePictureBytes()
            {
                ImgBytes = _fileService.GetImageByte(profilePicturePath),
                FileName = profilePicturePath
            };
        }

        public async Task DeleteProfilePicture(String userId)
        {
            String query = "update USER " +
                "set profile_picture_path = ? " + 
                "where user_id = ?;";

            using var conn = _dbConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = DBNull.Value });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task<bool> CheckIsAdmin(string userId)
        {
            int? response = null;

            String query = "select is_admin " +
                "from USER " + 
                "where user_id = ?;";

            using var conn = _dbConnection.GetConnection();
            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                response = int.Parse(reader["IS_ADMIN"].ToString()!);
            }
            
            if (response == null)
            {
                throw new UserNotFoundException();
            }

            if (IsAdmin.ADMIN.Equals((IsAdmin) response))
            {
                return true;
            }

            return false;
        }
    }
}