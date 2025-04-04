using api.DTO;
using api.Enum;
using api.Exceptions;
using api.Models;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface IReminderService
    {
        Task<List<ReminderResponse>> GetAllReminders(string userId);
        Task<List<ReminderResponse>> GetSpecificDateReminders(String userId, DateOnly date);
        Task<List<ReminderResponse>> GetSpecificMonthYearReminders(String userId, DateOnly monthYear);
        Task<ReminderResponse> GetReminderById(String reminderId, String userId);
        Task AddNewReminder(String userId, ParsedReminderRequest reminderRequest);
        Task UpdateReminder(ParsedUpdateReminderRequest updateReminderRequest, String userId);
        Task DeleteReminder(String userId, String reminderId);
    }

    public class ReminderService : IReminderService
    {
        private readonly DbConnection _dbConnection;
        private readonly IPriorityService _priorityService;

        public ReminderService(DbConnection dbConnection, IPriorityService priorityService)
        {
            _dbConnection = dbConnection;
            _priorityService = priorityService;
        }

        public async Task<List<ReminderResponse>> GetAllReminders(String userId)
        {
            List<ReminderResponse> reminders = new List<ReminderResponse>();

            String query = "select reminder_id, p.priority_id, p.name, title, deadline_date, start_time, end_time, description, is_done, type " +
                "from REMINDER r " +
                "join PRIORITY p on p.priority_id = r.priority_id " +
                "where user_id = ?;";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ReminderResponse reminder = new ReminderResponse()
                {
                    ReminderId = reader["REMINDER_ID"].ToString()!,
                    PriorityId = int.Parse(reader["PRIORITY_ID"].ToString()!),
                    PriorityName = reader["NAME"].ToString()!,
                    Title = reader["TITLE"].ToString()!,
                    DeadlineDate = DateOnly.FromDateTime(DateTime.Parse(reader["DEADLINE_DATE"].ToString()!)),
                    StartTime = DateTime.Parse(reader["START_TIME"].ToString()!),
                    EndTime = reader["END_TIME"] == DBNull.Value ? null : DateTime.Parse(reader["END_TIME"].ToString()!),
                    Description = reader["DESCRIPTION"].ToString()!,
                    IsDone = reader["IS_DONE"] == DBNull.Value ? null : (IsDone) int.Parse(reader["IS_DONE"].ToString()!),
                    Type = (ReminderType) int.Parse(reader["TYPE"].ToString()!)
                };

                reminders.Add(reminder);
            }

            return reminders.OrderBy(r => r.IsDone).ThenBy(r => r.DeadlineDate).ThenByDescending(r => r.PriorityId).ToList();
        }

        public async Task<List<ReminderResponse>> GetSpecificDateReminders(String userId, DateOnly date)
        {
            List<ReminderResponse> reminders = new List<ReminderResponse>();

            String query = "select reminder_id, p.priority_id, p.name, title, deadline_date, start_time, end_time, description, is_done, type " +
                "from REMINDER r " +
                "join PRIORITY p on p.priority_id = r.priority_id " +
                "where user_id = ? and deadline_date = ?";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = date });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ReminderResponse reminder = new ReminderResponse()
                {
                    ReminderId = reader["REMINDER_ID"].ToString()!,
                    PriorityId = int.Parse(reader["PRIORITY_ID"].ToString()!),
                    PriorityName = reader["NAME"].ToString()!,
                    Title = reader["TITLE"].ToString()!,
                    DeadlineDate = DateOnly.FromDateTime(DateTime.Parse(reader["DEADLINE_DATE"].ToString()!)),
                    StartTime = DateTime.Parse(reader["START_TIME"].ToString()!),
                    EndTime = reader["END_TIME"] == DBNull.Value ? null : DateTime.Parse(reader["END_TIME"].ToString()!),
                    Description = reader["DESCRIPTION"].ToString()!,
                    IsDone = reader["IS_DONE"] == DBNull.Value ? null : (IsDone) int.Parse(reader["IS_DONE"].ToString()!),
                    Type = (ReminderType) int.Parse(reader["TYPE"].ToString()!)
                };

                reminders.Add(reminder);
            }

            return reminders.OrderBy(r => r.IsDone).ThenBy(r => r.DeadlineDate).ThenByDescending(r => r.PriorityId).ToList();
        }

        public async Task<List<ReminderResponse>> GetSpecificMonthYearReminders(String userId, DateOnly monthYear)
        {
            List<ReminderResponse> reminders = new List<ReminderResponse>();

            String query = "select reminder_id, p.priority_id, p.name, title, deadline_date, start_time, end_time, description, is_done, type " +
                "from REMINDER r " +
                "join PRIORITY p on p.priority_id = r.priority_id " +
                "where user_id = ? and MONTH(deadline_date) = ? and YEAR(deadline_date) = ?;";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = monthYear.Month });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = monthYear.Year });

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                ReminderResponse reminder = new ReminderResponse()
                {
                    ReminderId = reader["REMINDER_ID"].ToString()!,
                    PriorityId = int.Parse(reader["PRIORITY_ID"].ToString()!),
                    PriorityName = reader["NAME"].ToString()!,
                    Title = reader["TITLE"].ToString()!,
                    DeadlineDate = DateOnly.FromDateTime(DateTime.Parse(reader["DEADLINE_DATE"].ToString()!)),
                    StartTime = DateTime.Parse(reader["START_TIME"].ToString()!),
                    EndTime = reader["END_TIME"] == DBNull.Value ? null : DateTime.Parse(reader["END_TIME"].ToString()!),
                    Description = reader["DESCRIPTION"].ToString()!,
                    IsDone = reader["IS_DONE"] == DBNull.Value ? null : (IsDone) int.Parse(reader["IS_DONE"].ToString()!),
                    Type = (ReminderType) int.Parse(reader["TYPE"].ToString()!)
                };

                reminders.Add(reminder);
            }

            return reminders.OrderBy(r => r.IsDone).ThenBy(r => r.DeadlineDate).ThenByDescending(r => r.PriorityId).ToList();
        }

        public async Task<ReminderResponse> GetReminderById(String reminderId, String userId)
        {
            ReminderResponse? reminderResponse = null;

            String query = "select reminder_id, p.priority_id, p.name, title, deadline_date, start_time, end_time, description, is_done, type " +
                "from REMINDER r " +
                "join PRIORITY p on p.priority_id = r.priority_id " +
                "where user_id = ? and reminder_id = ?";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = reminderId });

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                ReminderResponse reminder = new ReminderResponse()
                {
                    ReminderId = reader["REMINDER_ID"].ToString()!,
                    PriorityId = int.Parse(reader["PRIORITY_ID"].ToString()!),
                    PriorityName = reader["NAME"].ToString()!,
                    Title = reader["TITLE"].ToString()!,
                    DeadlineDate = DateOnly.FromDateTime(DateTime.Parse(reader["DEADLINE_DATE"].ToString()!)),
                    StartTime = DateTime.Parse(reader["START_TIME"].ToString()!),
                    EndTime = reader["END_TIME"] == DBNull.Value ? null : DateTime.Parse(reader["END_TIME"].ToString()!),
                    Description = reader["DESCRIPTION"].ToString()!,
                    IsDone = reader["IS_DONE"] == DBNull.Value ? null : (IsDone) int.Parse(reader["IS_DONE"].ToString()!),
                    Type = (ReminderType) int.Parse(reader["TYPE"].ToString()!)
                };

                reminderResponse = reminder;
            }

            if (reminderResponse == null)
            {
                throw new ReminderNotFoundException();
            }

            return reminderResponse;
        }

        public async Task AddNewReminder(String userId, ParsedReminderRequest reminderRequest)
        {
            List<Priority> priorities = _priorityService.GetAllPriorities();

            if (!priorities.Any(p => p.PriorityId == reminderRequest.PriorityId))
            {
                throw new PriorityIdNotExistException();
            }

            if (reminderRequest.EndTime != null && reminderRequest.StartTime >= reminderRequest.EndTime)
            {
                throw new StartTimeExceedEndTimeException();
            }

            String query = "insert into REMINDER (reminder_id, user_id, priority_id, title, deadline_date, start_time, end_time, description, is_done, type) " +
                "values (?, ?, ?, ?, ?, ?, ?, ?, ?, ?) ";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = Guid.NewGuid().ToString() });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = reminderRequest.PriorityId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = reminderRequest.Title });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = reminderRequest.DeadlineDate });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = reminderRequest.StartTime });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = (object?) reminderRequest.EndTime ?? DBNull.Value });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = reminderRequest.Description });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = reminderRequest.Type.Equals(ReminderType.TASK) ? (object) IsDone.UNDONE : DBNull.Value });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = reminderRequest.Type });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task UpdateReminder(ParsedUpdateReminderRequest updateReminderRequest, String userId)
        {
            if (updateReminderRequest.Type.Equals(ReminderType.TASK) && updateReminderRequest.IsDone == null)
            {
                throw new DoneStatusCannotEmptyException();
            } else if (updateReminderRequest.Type.Equals(ReminderType.REMINDER) && updateReminderRequest.IsDone != null)
            {
                updateReminderRequest.IsDone = null;
            }

            if (updateReminderRequest.EndTime != null && updateReminderRequest.StartTime >= updateReminderRequest.EndTime)
            {
                throw new StartTimeExceedEndTimeException();
            }

            String query = "update REMINDER " +
                "set priority_id = ?, title = ?, deadline_date = ?, start_time = ?, end_time = ?, description = ?, is_done = ?, type = ? " + 
                "where reminder_id = ? and user_id = ?;";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
        
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = updateReminderRequest.PriorityId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updateReminderRequest.Title });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = updateReminderRequest.DeadlineDate });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = updateReminderRequest.StartTime });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Timestamp, Value = (object?) updateReminderRequest.EndTime ?? DBNull.Value });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updateReminderRequest.Description });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = (object?) updateReminderRequest.IsDone ?? DBNull.Value });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = updateReminderRequest.Type });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updateReminderRequest.ReminderId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });
        
            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }

        public async Task DeleteReminder(String userId, String reminderId)
        {
            String query = "delete from REMINDER " +
                "where reminder_id = ? and user_id = ?;";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = reminderId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = userId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }
        }
    }
}