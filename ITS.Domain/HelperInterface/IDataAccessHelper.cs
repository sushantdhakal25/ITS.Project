
namespace ITS.Domain.HelperInterface
{
    public interface IDataAccessHelper
    {
        Task<string> ExecuteActionProcedureAsync(string storedProcedureName, IDictionary<string, object> inputParameters, string? outputParameterName = "");
        Task<List<T>> ExecuteRetrievalProcedureAsync<T>(string storedProcedureName, IDictionary<string, object> inputParameters) where T : class;
    }
}
