using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO;
using api.Models;
using LazyCache;
using MySql.Data.MySqlClient;

namespace api.Services
{
    public interface IPriorityService
    {
        List<Priority> GetAllPriorities();
        Task AddNewPriority(Priority newPriority, String userId);
        Task UpdatePriority(UpdatePriorityRequest updatePriorityRequest, String userId);
        Task DeletePriority(int priorityId, String userId);
    }

    public class PriorityService : IPriorityService
    {
        private readonly IUserService _userService;
        private readonly DbConnection _dbConnection;
        private readonly IAppCache _cacheService = new CachingService();

        public PriorityService(IUserService userService, DbConnection dbConnection)
        {
            _userService = userService;
            _dbConnection = dbConnection;
        }

        public async Task AddNewPriority(Priority newPriority, String userId)
        {
            if (!await _userService.CheckIsAdmin(userId))
            {
                throw new UnauthorizedAccessException();
            }

            String query = "insert into PRIORITY (priority_id, name) " +
                "values (?, ?);";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = newPriority.PriorityId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = newPriority.Name });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }

            _cacheService.Remove("PRIORITIES_LIST");
        }

        public async Task DeletePriority(int priorityId, String userId)
        {
            if (!await _userService.CheckIsAdmin(userId))
            {
                throw new UnauthorizedAccessException();
            }

            String query = "delete from PRIORITY " +
                "where priority_id = ?;";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = priorityId });

            int res = await cmd.ExecuteNonQueryAsync();

            if (res == 0)
            {
                throw new Exception();
            }

            _cacheService.Remove("PRIORITIES_LIST");
        }

        public List<Priority> GetAllPriorities()
        {
            Func<Task<List<Priority>>> priorityFactory = async () => await PopulatePriorities();

            List<Priority> priorities = _cacheService.GetOrAdd("PRIORITIES_LIST", priorityFactory, DateTimeOffset.UtcNow.AddHours(2)).Result;

            return priorities;
        }

        private async Task<List<Priority>> PopulatePriorities()
        {
            List<Priority> priorities = new List<Priority>();

            String query = "select priority_id, name " +
                "from PRIORITY;";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                Priority priority = new Priority()
                {
                    PriorityId = int.Parse(reader["PRIORITY_ID"].ToString()!),
                    Name = reader["Name"].ToString()!
                };

                priorities.Add(priority);
            }

            return priorities.OrderBy(p => p.PriorityId).ToList();
        }

        public async Task UpdatePriority(UpdatePriorityRequest updatePriorityRequest, String userId)
        {
            if (!await _userService.CheckIsAdmin(userId))
            {
                throw new UnauthorizedAccessException();
            }

            String query = "update PRIORITY " +
                "set priority_id = ?, name = ? " + 
                "where priority_id = ?";

            using MySqlConnection conn = _dbConnection.GetConnection();
            using MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = updatePriorityRequest.NewPriorityId });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.VarChar, Value = updatePriorityRequest.Name });
            cmd.Parameters.Add(new MySqlParameter() { MySqlDbType = MySqlDbType.Int32, Value = updatePriorityRequest.PriorityId });

            int resp = await cmd.ExecuteNonQueryAsync();

            _cacheService.Remove("PRIORITIES_LIST");
        }
    }
}