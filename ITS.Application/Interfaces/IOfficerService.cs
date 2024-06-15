
using ITS.Domain.Entities;

namespace ITS.Application.Interfaces
{
    public interface IOfficerService
    {
        Task<string> GetOfficersAsync(string? searchText);
        Task<Officer> GetOfficerLoginAsync(string identificationNumber, string password);
        Task<string> AddOfficerAsync(string param);
        Task<string> UpdateOfficerAsync(string param);
        Task<string> DeleteOfficerAsync(string param);
    }
}
