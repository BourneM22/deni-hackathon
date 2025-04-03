using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO;
using api.Exceptions;
using api.Models;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface ISoundbardService
    {
        Task<List<SoundboardResponse>> GetAllSoundboard(String userId);
        Task<List<SoundboardResponse>> GetAllSoundboardByFilterId(String userId, String sbFilterId);
        Task<SoundboardResponse> GetSoundboardById(String userId, String soundboardId);
        Task UpdateSoundboard(String userId, UpdateSoundboardRequest updateSoundboardRequest);
        Task DeleteSoundboard(String userId, String soundboardId);
        Task AddNewSoundBoard(String userId, SoundboardRequest soundboardRequest);
    }

    public class SoundboardService : ISoundbardService
    {
        private readonly DbConnection _dbConnection;

        public SoundboardService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddNewSoundBoard(string userId, SoundboardRequest soundboardRequest)
        {
            String query = "insert into SOUNDBOARD (sound_id, user_id, filter_id, name, description) " +
                "values (?, ?, ?, ?, ?);";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = Guid.NewGuid().ToString() });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = (object?) soundboardRequest.FilterId ?? DBNull.Value });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = soundboardRequest.Name });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = soundboardRequest.Description });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task DeleteSoundboard(string userId, string soundboardId)
        {
            String query = "delete from SOUNDBOARD " +
                "where sound_id = ? and user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = soundboardId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task<List<SoundboardResponse>> GetAllSoundboard(string userId)
        {
            List<SoundboardResponse> soundboards = new List<SoundboardResponse>();

            String query = "select sound_id, filter_id, name, description " +
                "from SOUNDBOARD " + 
                "where user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                if (reader["FILTER_ID"] == DBNull.Value)
                {
                    SoundboardResponse soundboard = new SoundboardResponse()
                    {
                        SoundId = reader["SOUND_ID"].ToString()!,
                        FilterId = null,
                        Name = reader["NAME"].ToString()!,
                        Description =  reader["DESCRIPTION"].ToString()!,
                    };

                    soundboards.Add(soundboard);
                } else
                {
                    SoundboardResponse soundboard = new SoundboardResponse()
                    {
                        SoundId = reader["SOUND_ID"].ToString()!,
                        FilterId = reader["FILTER_ID"].ToString()!,
                        Name = reader["NAME"].ToString()!,
                        Description =  reader["DESCRIPTION"].ToString()!,
                    };

                    soundboards.Add(soundboard);
                }
            }

            return soundboards;
        }

        public async Task<List<SoundboardResponse>> GetAllSoundboardByFilterId(string userId, string sbFilterId)
        {
            List<SoundboardResponse> soundboards = new List<SoundboardResponse>();

            String query = "select sound_id, filter_id, name, description " +
                "from SOUNDBOARD " + 
                "where user_id = ? and filter_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = sbFilterId });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                SoundboardResponse soundboard = new SoundboardResponse()
                {
                    SoundId = reader["SOUND_ID"].ToString()!,
                    FilterId = reader["FILTER_ID"].ToString()!,
                    Name = reader["NAME"].ToString()!,
                    Description =  reader["DESCRIPTION"].ToString()!,
                };

                soundboards.Add(soundboard);
            }

            return soundboards;
        }

        public async Task<SoundboardResponse> GetSoundboardById(string userId, string soundboardId)
        {
            SoundboardResponse? soundboard = null;

            String query = "select sound_id, filter_id, name, description " +
                "from SOUNDBOARD " + 
                "where user_id = ? and sound_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = soundboardId });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                SoundboardResponse temp = new SoundboardResponse()
                {
                    SoundId = reader["SOUND_ID"].ToString()!,
                    FilterId = reader["FILTER_ID"].ToString()!,
                    Name = reader["NAME"].ToString()!,
                    Description =  reader["DESCRIPTION"].ToString()!,
                };

                soundboard = temp;
            }

            if (soundboard == null)
            {
                throw new SoundboardDoesNotExistException();
            }

            return soundboard;
        }

        public async Task UpdateSoundboard(string userId, UpdateSoundboardRequest updateSoundboardRequest)
        {
            String query = "update SOUNDBOARD " + 
                "set filter_id = ?, name = ?, description = ? " + 
                "where sound_id = ? and user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = (object?) updateSoundboardRequest.FilterId ?? DBNull.Value });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updateSoundboardRequest.Name });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updateSoundboardRequest.Description });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updateSoundboardRequest.SoundId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }
    }
}