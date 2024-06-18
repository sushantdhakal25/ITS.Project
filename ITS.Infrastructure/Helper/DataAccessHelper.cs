using ITS.Domain.HelperInterface;
using ITS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.Infrastructure.Helper
{
    public class DataAccessHelper : IDataAccessHelper
    {
        private readonly ApplicationDbContext _context;

        public DataAccessHelper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string?> ExecuteActionProcedureAsync(string storedProcedureName, IDictionary<string, object> inputParameters, string? outputParameterName = "")
        {
            var parameters = new List<MySqlParameter>();
            foreach (var param in inputParameters)
            {
                parameters.Add(new MySqlParameter(param.Key, param.Value));
            }
            MySqlParameter? outputParam = null;
            if (!string.IsNullOrEmpty(outputParameterName))
            {
                outputParam = new MySqlParameter(outputParameterName, MySqlDbType.LongText)
                {
                    Direction = ParameterDirection.Output
                };

                parameters.Add(outputParam);
            }
            string parameterPlaceholders = string.Join(", ", parameters.Select(p => p.ParameterName));
            var sqlParams = parameters.ToArray();
            Console.WriteLine($"CALL {storedProcedureName}({parameterPlaceholders})");
            await _context.Database.ExecuteSqlRawAsync($"CALL {storedProcedureName}({parameterPlaceholders})", sqlParams);

            return outputParam?.Value?.ToString();
        }

        public async Task<List<T>> ExecuteRetrievalProcedureAsync<T>(string storedProcedureName, IDictionary<string, object> inputParameters) where T : class
        {
            var parameters = new List<MySqlParameter>();
            foreach (var param in inputParameters)
            {
                parameters.Add(new MySqlParameter(param.Key, param.Value));
            }

            var parameterPlaceholders = string.Join(", ", parameters.Select(p => p.ParameterName));
            var sqlParams = parameters.ToArray();
            Console.WriteLine($"CALL {storedProcedureName}({parameterPlaceholders})");

            var result = await _context.Set<T>().FromSqlRaw($"CALL {storedProcedureName}({parameterPlaceholders})", sqlParams).ToListAsync();
            return result;
        }
    }

}
