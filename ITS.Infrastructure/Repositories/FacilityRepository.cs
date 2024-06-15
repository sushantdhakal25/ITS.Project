using ITS.Domain.Entities;
using ITS.Domain.HelperInterface;
using ITS.Domain.Repositories;

namespace ITS.Infrastructure.Repositories
{
    public class FacilityRepository : IFacilityRepository
    {
        private readonly IDataAccessHelper _dataAccessHelper;

        public FacilityRepository(IDataAccessHelper dataAccessHelper)
        {
            _dataAccessHelper = dataAccessHelper;
        }

        public async Task<string> GetAsync(string? searchText)
        {

            var inputParams = new Dictionary<string, object>
            {
                { "@searchText", searchText }
            };

            var jsonResult = await _dataAccessHelper.ExecuteRetrievalProcedureAsync<MvResult>("SpFacilitySel", inputParams);
            return jsonResult.FirstOrDefault()?.Json ?? string.Empty;
        }

        public async Task<string> AddAsync(string param)
        {
            var inputParams = new Dictionary<string, object>
            {
               { "@param", param }
            };

            string result = await _dataAccessHelper.ExecuteActionProcedureAsync("SpFacilityInsert", inputParams, "@result");
            return result;
        }

        public async Task<string> UpdateAsync(string param)
        {
            var inputParams = new Dictionary<string, object>
            {
               { "@param", param }
            };

            string result = await _dataAccessHelper.ExecuteActionProcedureAsync("SpFacilityUpdate", inputParams, "@result");
            return result;
        }

        public async Task<string> DeleteAsync(string param)
        {
            var inputParams = new Dictionary<string, object>
            {
               { "@param", param }
            };

            string result = await _dataAccessHelper.ExecuteActionProcedureAsync("SpFacilityDelete", inputParams, "@result");
            return result;
        }
    }
}
