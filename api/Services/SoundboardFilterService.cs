using api.DTO;
using api.Models;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface ISoundboardFilterService
    {
        Task<List<SoundboardFilterResponse>> GetAllSounboardFilters(String userId);
        Task UpdateSounboardFilter(String userId, UpdateSoundboardFilterRequest updateSbFilterReq);
        Task AddNewSoundboardFilter(String userId, SoundboardFilterRequest soundboardFilterRequest);
        Task DeleteSoundboardFilter(String userId, String soundboardFilterId);
    }

    public class SoundboardFilterService : ISoundboardFilterService
    {
        private readonly DbConnection _dbConnection;

        public SoundboardFilterService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddNewSoundboardFilter(string userId, SoundboardFilterRequest soundboardFilterRequest)
        {
            String query = "insert into SOUNDBOARD_FILTER (filter_id, user_id, name) " +
                "values (?, ?, ?);";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = Guid.NewGuid().ToString() });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = soundboardFilterRequest.Name });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task DeleteSoundboardFilter(string userId, string soundboardFilterId)
        {
            String query = "delete from SOUNDBOARD_FILTER " +
                "where filter_id = ? and user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = soundboardFilterId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task<List<SoundboardFilterResponse>> GetAllSounboardFilters(string userId)
        {
            List<SoundboardFilterResponse> filters = new List<SoundboardFilterResponse>();

            String query = "select filter_id, name " +
                "from SOUNDBOARD_FILTER " +
                "where user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                SoundboardFilterResponse filter = new SoundboardFilterResponse()
                {
                    FilterId = reader["FILTER_ID"].ToString()!,
                    Name = reader["NAME"].ToString()!
                };

                filters.Add(filter);
            }

            return filters;
        }

        public async Task UpdateSounboardFilter(string userId, UpdateSoundboardFilterRequest updateSbFilterReq)
        {
            String query = "update SOUNDBOARD_FILTER " +
                "set name = ? " +
                "where filter_id = ? and user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updateSbFilterReq.Name });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updateSbFilterReq.FilterId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }
    }
}