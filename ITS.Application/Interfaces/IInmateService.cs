
namespace ITS.Application.Interfaces
{
    public interface IInmateService
    {
        Task<string> GetInmatesAsync(string? searchText);
        Task<string> AddInmateAsync(string param);
        Task<string> UpdateInmateAsync(string param);
        Task<string> DeleteInmateAsync(string param);
    }
}
