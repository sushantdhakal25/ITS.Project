using ITS.Domain.Entities;
using ITS.Domain.HelperInterface;
using ITS.Domain.Repositories;

namespace ITS.Infrastructure.Repositories
{
    public class InmateRepository : IInmateRepository
    {
        private readonly IDataAccessHelper _dataAccessHelper;

        public InmateRepository(IDataAccessHelper dataAccessHelper)
        {
            _dataAccessHelper = dataAccessHelper;
        }

        public async Task<string> GetAsync(string? searchText)
        {
            //var parameter = new MySqlParameter("@searchText", searchText);
            //var jsonResult = await _context.Set<MvResult>()
            //                               .FromSqlRaw("CALL SpInmateSel(@searchText)", parameter)
            //                               .ToListAsync();

            //return jsonResult.FirstOrDefault()?.Json ?? string.Empty;

            var inputParams = new Dictionary<string, object>
            {
                { "@searchText", searchText }
            };

            var jsonResult = await _dataAccessHelper.ExecuteRetrievalProcedureAsync<MvResult>("SpInmateSel", inputParams);
            return jsonResult.FirstOrDefault()?.Json ?? string.Empty;
        }

        public async Task<string> AddAsync(string param)
        {
            //var inputParam = new MySqlParameter("@param", MySqlDbType.LongText)
            //{
            //    Direction = ParameterDirection.Input,
            //    Value = param
            //};

            //var outputParam = new MySqlParameter("@result", MySqlDbType.LongText)
            //{
            //    Direction = ParameterDirection.Output
            //};
            //await _context.Database.ExecuteSqlRawAsync("CALL SpInmateInsert(@param, @result)", inputParam, outputParam);
            //var jsonResponse = outputParam.Value?.ToString();
            //return jsonResponse ?? string.Empty;

            var inputParams = new Dictionary<string, object>
            {
               { "@param", param }
            };

            string result = await _dataAccessHelper.ExecuteActionProcedureAsync("SpInmateInsert", inputParams, "@result");
            return result;
        }

        public async Task<string> UpdateAsync(string param)
        {
            var inputParams = new Dictionary<string, object>
            {
               { "@param", param }
            };

            string result = await _dataAccessHelper.ExecuteActionProcedureAsync("SpInmateUpdate", inputParams, "@result");
            return result;
        }

        public async Task<string> DeleteAsync(string param)
        {
            var inputParams = new Dictionary<string, object>
            {
               { "@param", param }
            };

            string result = await _dataAccessHelper.ExecuteActionProcedureAsync("SpInmateDelete", inputParams, "@result");
            return result;
        }
    }
}
