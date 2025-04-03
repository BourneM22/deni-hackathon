using api.DTO;
using api.Exceptions;
using api.Models;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface INoteService
    {
        Task AddNewNote(NoteRequest newNoteRequest, String userId);
        Task UpdateNote(UpdateNoteRequest updatedNote, String userId);
        Task<List<NoteResponse>> GetAllNotes(String userId);
        Task<NoteResponse> GetNoteById(String userId, String noteId);
        Task<List<NoteResponse>> GetAllNotesByTagId(String userId, String noteTagId);
        Task DeleteNote(String noteId, String userId);
    }

    public class NoteService : INoteService
    {
        private readonly DbConnection _dbConnection;

        public NoteService(DbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddNewNote(NoteRequest newNoteRequest, String userId)
        {
            String query = "insert into NOTE (note_id, user_id, title, content, created_date_time, modified_date_time, tag_id) " +
                "values (?, ?, ?, ?, ?, ?, ?);";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = Guid.NewGuid().ToString() });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = newNoteRequest.Title });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = newNoteRequest.Content });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = DateTime.Now });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = DateTime.Now });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = (object?) newNoteRequest.TagId ?? DBNull.Value });

            int res = await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateNote(UpdateNoteRequest updatedNote, String userId)
        {
            String query = "update NOTE " +
                "set title = ?, content = ?, modified_date_time = ?, tag_id = ? " + 
                "where note_id = ? and user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updatedNote.Title });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updatedNote.Content });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = DateTime.Now });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = (object?) updatedNote.TagId ?? DBNull.Value });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updatedNote.NoteId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task<List<NoteResponse>> GetAllNotes(String userId)
        {
            List<NoteResponse> notes = new List<NoteResponse>();

            String query = "select note_id, title, content, modified_date_time, tag_id " +
                "from NOTE " + 
                "where user_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                NoteResponse note = new NoteResponse()
                {
                    NoteId = reader["NOTE_ID"].ToString()!,
                    Title = reader["TITLE"].ToString()!,
                    Content = reader["CONTENT"].ToString()!,
                    ModifiedDateTime = DateTime.Parse(reader["MODIFIED_DATE_TIME"].ToString()!),
                    TagId = reader["TAG_ID"] == DBNull.Value ? null : reader["TAG_ID"].ToString()
                };

                notes.Add(note);
            }

            return notes.OrderByDescending(n => n.ModifiedDateTime).ToList();
        }

        public async Task DeleteNote(String noteId, String userId)
        {
            String query = "delete from NOTE " +
                "where note_id = ? and user_id = ?;";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = noteId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task<NoteResponse> GetNoteById(string userId, string noteId)
        {
            NoteResponse? noteResponse = null;

            String query = "select note_id, title, content, modified_date_time, tag_id " +
                "from NOTE " + 
                "where user_id = ? and note_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = noteId });

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                NoteResponse note = new NoteResponse()
                {
                    NoteId = reader["NOTE_ID"].ToString()!,
                    Title = reader["TITLE"].ToString()!,
                    Content = reader["CONTENT"].ToString()!,
                    ModifiedDateTime = DateTime.Parse(reader["MODIFIED_DATE_TIME"].ToString()!),
                    TagId = reader["TAG_ID"] == DBNull.Value ? null : reader["TAG_ID"].ToString()
                };

                noteResponse = note;
            }

            if (noteResponse == null)
            {
                throw new NoteNotFoundException();
            }

            return noteResponse;
        }

        public async Task<List<NoteResponse>> GetAllNotesByTagId(String userId, String noteTagId)
        {
            List<NoteResponse> notes = new List<NoteResponse>();

            String query = "select note_id, title, content, modified_date_time, tag_id " +
                "from NOTE " + 
                "where user_id = ? and tag_id = ?;";
                
            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = noteTagId });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                NoteResponse note = new NoteResponse()
                {
                    NoteId = reader["NOTE_ID"].ToString()!,
                    Title = reader["TITLE"].ToString()!,
                    Content = reader["CONTENT"].ToString()!,
                    ModifiedDateTime = DateTime.Parse(reader["MODIFIED_DATE_TIME"].ToString()!),
                    TagId = reader["TAG_ID"] == DBNull.Value ? null : reader["TAG_ID"].ToString()
                };

                notes.Add(note);
            }

            return notes.OrderByDescending(n => n.ModifiedDateTime).ToList();
        }
    }
}