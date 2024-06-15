using ITS.Domain.Entities;
using ITS.Domain.HelperInterface;
using ITS.Domain.Repositories;

namespace ITS.Infrastructure.Repositories
{
    public class OfficerRepository : IOfficerRepository
    {
        private readonly IDataAccessHelper _dataAccessHelper;

        public OfficerRepository(IDataAccessHelper dataAccessHelper)
        {
            _dataAccessHelper = dataAccessHelper;
        }

        public async Task<string> GetAsync(string? searchText)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "@searchText", searchText }
            };

            var jsonResult = await _dataAccessHelper.ExecuteRetrievalProcedureAsync<MvResult>("SpOfficerSel", inputParams);
            return jsonResult.FirstOrDefault()?.Json ?? string.Empty;
        }

        public async Task<Officer> GetOfficerLoginAsync(string identificationNumber, string password)
        {
            var inputParams = new Dictionary<string, object>
            {
                { "@identificationNumber", identificationNumber },
                { "@password", password }
            };

            var jsonResult = await _dataAccessHelper.ExecuteRetrievalProcedureAsync<Officer>("SpOfficerLoginSel", inputParams);
            return jsonResult.FirstOrDefault();
        }

        public async Task<string> AddAsync(string param)
        {
            var inputParams = new Dictionary<string, object>
            {
               { "@param", param }
            };

            string result = await _dataAccessHelper.ExecuteActionProcedureAsync("SpOfficerInsert", inputParams, "@result");
            return result;
        }

        public async Task<string> UpdateAsync(string param)
        {
            var inputParams = new Dictionary<string, object>
            {
               { "@param", param }
            };

            string result = await _dataAccessHelper.ExecuteActionProcedureAsync("SpOfficerUpdate", inputParams, "@result");
            return result;
        }

        public async Task<string> DeleteAsync(string param)
        {
            var inputParams = new Dictionary<string, object>
            {
               { "@param", param }
            };

            string result = await _dataAccessHelper.ExecuteActionProcedureAsync("SpOfficerDelete", inputParams, "@result");
            return result;
        }
    }
}
