using api.DTO;
using api.Exceptions;
using api.Models;
using Microsoft.Extensions.Options;
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
        Task<List<NoteResponse>> SearchNoteByKeyword(String userId, String keyword);
    }

    public class NoteService : INoteService
    {
        private readonly DbConnection _dbConnection;
        private readonly SearchingConfig _searchingConfig;
        private readonly ISearchingService _searchingService;
        private readonly INoteTagService _noteTagService;

        public NoteService(DbConnection dbConnection, IOptions<SearchingConfig> searchingConfig, ISearchingService searchingService, INoteTagService noteTagService)
        {
            _dbConnection = dbConnection;
            _searchingConfig = searchingConfig.Value;
            _searchingService = searchingService;
            _noteTagService = noteTagService;
        }

        public async Task AddNewNote(NoteRequest newNoteRequest, String userId)
        {
            List<NoteTagResponse> tags = await _noteTagService.GetAllNoteTags(userId);

            if (!tags.Any(t => t.TagId == newNoteRequest.TagId))
            {
                throw new TagIdNotFoundException();
            }

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
            List<NoteTagResponse> tags = await _noteTagService.GetAllNoteTags(userId);

            if (!tags.Any(t => t.TagId == updatedNote.TagId))
            {
                throw new TagIdNotFoundException();
            }

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

        public async Task<List<NoteResponse>> SearchNoteByKeyword(String userId, String search)
        {
            List<NoteResponse> notes = await GetAllNotes(userId);
            List<Tuple<NoteResponse, Double>> results = new List<Tuple<NoteResponse, double>>();

            search = search.ToLower();
            Double mismatchTolerance =_searchingConfig.MismatchTolerance;

            foreach (NoteResponse note in notes)
            {
                Double titleScore = _searchingService.ComputeCosineSimilarity(search, note.Title.ToLower());
                Double contentScore = _searchingService.ComputeCosineSimilarity(search, note.Content.ToLower());

                long totalLength = note.Title.Length + note.Content.Length;
                Double newTolerance = mismatchTolerance / (Double) totalLength;

                if (titleScore >= newTolerance || contentScore >= newTolerance)
                {
                    results.Add(new Tuple<NoteResponse, Double>(note, titleScore + contentScore));
                }
            }

            results.Sort((a, b) => b.Item2.CompareTo(a.Item2));

            return results.Select(r => r.Item1).ToList();
        }
    }
}