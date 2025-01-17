using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace Common.Persistence
{
    public class Dal : IDal
    {
        private readonly DbDetail _db;

        public Dal(DbDetail db)
        {
            _db = db;
        }

        private SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_db.ConnectionString);
            return conn;
        }

        public async Task<Tuple<SqlConnection, GridReader>> RunMultipleQueryAsync(string query, CommandType commandType, DynamicParameters? parameters = null, int timeOut = 60)
        {
            var conn = GetConnection();
            var queryResult = await conn.QueryMultipleAsync(query, parameters, commandType: commandType, commandTimeout: timeOut);
            return Tuple.Create(conn, queryResult);
        }

        public async Task<IEnumerable<T>> GetIEnumerableData<T>(string query, CommandType commandType, DynamicParameters? parameters = null, int timeOut = 60) where T : class
        {
            using (var conn = GetConnection())
            {
                var result = await conn.QueryAsync<T>(query, parameters, commandType: commandType, commandTimeout: timeOut);
                return result;
            }
        }

        public async Task<T> GetData<T>(string query, CommandType commandType = CommandType.StoredProcedure, DynamicParameters? parameters = null, int timeOut = 60) where T : class
        {
            using (var conn = GetConnection())
            {
                var result = await conn.QueryFirstOrDefaultAsync<T>(query, parameters, commandType: commandType, commandTimeout: timeOut);
                return result;
            }
        }

        public async Task<int> GetScalarData(string query, CommandType commandType = CommandType.StoredProcedure, DynamicParameters? parameters = null, int timeOut = 60)
        {
            using (var conn = GetConnection())
            {
                var result = await conn.ExecuteScalarAsync<int>(query, parameters, commandType: commandType, commandTimeout: timeOut);
                return result;
            }
        }

        public async Task<long> GetScalarLongData(string query, CommandType commandType = CommandType.StoredProcedure, DynamicParameters? parameters = null, int timeOut = 60)
        {
            using (var conn = GetConnection())
            {
                var result = await conn.ExecuteScalarAsync<int>(query, parameters, commandType: commandType, commandTimeout: timeOut);
                return result;
            }
        }

        public async Task<string> GetScalarStringData(string query, CommandType commandType = CommandType.StoredProcedure, DynamicParameters? parameters = null, int timeOut = 60)
        {
            using (var conn = GetConnection())
            {
                var result = await conn.ExecuteScalarAsync<string>(query, parameters, commandType: commandType, commandTimeout: timeOut);
                return result;
            }
        }

        public async Task<int> ExecuteQuery(string query, CommandType commandType = CommandType.StoredProcedure, DynamicParameters? parameters = null, int timeOut = 60)
        {
            using (var conn = GetConnection())
            {
                var result = await conn.ExecuteAsync(query, parameters, commandType: commandType, commandTimeout: timeOut);
                return result;
            }
        }

        public async Task SQLBulkUpload(DataTable dataTable, List<string> columnMapping, string destinationTable, DynamicParameters? parameters = null, int timeOut = 60)
        {
            using (var conn = GetConnection())
            {
                var bulkCopy = new SqlBulkCopy(conn);

                bulkCopy.BulkCopyTimeout = 900;
                bulkCopy.BatchSize = 4000;
                bulkCopy.DestinationTableName = destinationTable;

                // Add your column mappings here
                foreach (var mapping in columnMapping)
                {
                    var split = mapping.Split(new[] { ',' });
                    bulkCopy.ColumnMappings.Add(split.First(), split.Last());
                }

                await conn.OpenAsync();
                bulkCopy.EnableStreaming = false;
                bulkCopy.WriteToServer(dataTable);
                await conn.CloseAsync();
            }
        }
    }
}
