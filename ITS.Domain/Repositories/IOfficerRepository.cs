using ITS.Domain.Entities;

namespace ITS.Domain.Repositories
{
    public interface IOfficerRepository
    {
        Task<string> GetAsync(string? searchText);
        Task<Officer> GetOfficerLoginAsync(string identificationNumber, string password);
        Task<string> AddAsync(string param);
        Task<string> UpdateAsync(string param);
        Task<string> DeleteAsync(string param);
    }
}
