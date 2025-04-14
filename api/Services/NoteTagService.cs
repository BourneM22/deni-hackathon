using api.DTO;
using api.Models;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface INoteTagService
    {
        Task<List<NoteTagResponse>> GetAllNoteTags(String userId);
        Task UpdateNoteTag(String userId, UpdateNoteTagRequest updateNoteTagRequest);
        Task AddNewNoteTag(String userId, NoteTagRequest noteTagRequest);
        Task DeleteNoteTag(String userId, String noteTagId);
    }

    public class NoteTagService : INoteTagService
    {
        private readonly DbConnection _dbConnection;

        public NoteTagService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddNewNoteTag(String userId, NoteTagRequest noteTagRequest)
        {
            String query = "insert into NOTE_TAG (tag_id, user_id, name) " +
                "values (?, ?, ?);";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = Guid.NewGuid().ToString() });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = noteTagRequest.Name });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task DeleteNoteTag(String userId, String noteTagId)
        {
            String query = "delete from NOTE_TAG " +
                "where tag_id = ? and user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = noteTagId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task<List<NoteTagResponse>> GetAllNoteTags(String userId)
        {
            List<NoteTagResponse> tags = new List<NoteTagResponse>();

            String query = "select tag_id, name " +
                "from NOTE_TAG " +
                "where user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                NoteTagResponse tag = new NoteTagResponse()
                {
                    TagId = reader["TAG_ID"].ToString()!,
                    Name = reader["NAME"].ToString()!
                };

                tags.Add(tag);
            }

            return tags;
        }

        public async Task UpdateNoteTag(String userId, UpdateNoteTagRequest updateNoteTagRequest)
        {
            String query = "update NOTE_TAG " +
                "set name = ? " +
                "where tag_id = ? and user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updateNoteTagRequest.Name });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updateNoteTagRequest.TagId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }
    }
}