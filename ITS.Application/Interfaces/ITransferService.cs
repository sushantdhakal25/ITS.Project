
namespace ITS.Application.Interfaces
{
    public interface ITransferService
    {
        Task<string> GetTransfersAsync(string? searchText);
        Task<string> AddTransferAsync(string param);
        Task<string> UpdateTransferAsync(string param);
        Task<string> DeleteTransferAsync(string param);
    }
}
