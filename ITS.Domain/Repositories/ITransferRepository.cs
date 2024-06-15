
namespace ITS.Domain.Repositories
{
    public interface ITransferRepository
    {
        Task<string> GetAsync(string? searchText);
        Task<string> AddAsync(string param);
        Task<string> UpdateAsync(string param);
        Task<string> DeleteAsync(string param);
    }
}
