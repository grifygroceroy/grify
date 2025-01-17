using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Persistence
{
    public interface IDal
    {
        public Task<int> ExecuteQuery(string query, CommandType commandType = CommandType.StoredProcedure, DynamicParameters? parameters = null, int timeOut = 60);

        public Task<T> GetData<T>(string query, CommandType commandType = CommandType.StoredProcedure, DynamicParameters? parameters = null, int timeOut = 60) where T : class;

        public Task<IEnumerable<T>> GetIEnumerableData<T>(string query, CommandType commandType, DynamicParameters? parameters = null, int timeOut = 60) where T : class;

        public Task<int> GetScalarData(string query, CommandType commandType = CommandType.StoredProcedure, DynamicParameters? parameters = null, int timeOut = 60);

        public Task<string> GetScalarStringData(string query, CommandType commandType = CommandType.StoredProcedure, DynamicParameters? parameters = null, int timeOut = 60);

        Task<long> GetScalarLongData(string query, CommandType commandType = CommandType.StoredProcedure, DynamicParameters? parameters = null, int timeOut = 60);

        public Task<Tuple<SqlConnection, SqlMapper.GridReader>> RunMultipleQueryAsync(string query, CommandType commandType, DynamicParameters? parameters = null, int timeOut = 60);

        public Task SQLBulkUpload(DataTable dataTable, List<string> columnMapping, string destinationTable, DynamicParameters? parameters = null, int timeOut = 60);
    }
}
