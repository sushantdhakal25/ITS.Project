using ITS.Application.Interfaces;
using ITS.Domain.Repositories;

namespace ITS.Application.Services
{
    public class TransferService : ITransferService
    {
        private readonly ITransferRepository _transferRepository;

        public TransferService(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        public async Task<string> GetTransfersAsync(string? searchText)
        {
            return await _transferRepository.GetAsync(searchText);
        }

        public async Task<string> AddTransferAsync(string param)
        {
            return await _transferRepository.AddAsync(param);
        }

        public async Task<string> UpdateTransferAsync(string param)
        {
            return await _transferRepository.UpdateAsync(param);
        }

        public async Task<string> DeleteTransferAsync(string param)
        {
            return await _transferRepository.DeleteAsync(param);
        }

    }
}
