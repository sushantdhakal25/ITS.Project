using ITS.Application.Interfaces;
using ITS.Domain.Repositories;

namespace ITS.Application.Services
{
    public class InmateService : IInmateService
    {
        private readonly IInmateRepository _inmateRepository;

        public InmateService(IInmateRepository inmateRepository)
        {
            _inmateRepository = inmateRepository;
        }

        public async Task<string> GetInmatesAsync(string? searchText)
        {
            return await _inmateRepository.GetAsync(searchText);
        }

        public async Task<string> AddInmateAsync(string param)
        {
            return await _inmateRepository.AddAsync(param);
        }

        public async Task<string> UpdateInmateAsync(string param)
        {
            return await _inmateRepository.UpdateAsync(param);
        }

        public async Task<string> DeleteInmateAsync(string param)
        {
            return await _inmateRepository.DeleteAsync(param);
        }

    }
}
