using ITS.Application.Interfaces;
using ITS.Domain.Entities;
using ITS.Domain.Repositories;

namespace ITS.Application.Services
{
    public class OfficerService : IOfficerService
    {
        private readonly IOfficerRepository _officerRepository;

        public OfficerService(IOfficerRepository officerRepository)
        {
            _officerRepository = officerRepository;
        }

        public async Task<string> GetOfficersAsync(string? searchText)
        {
            return await _officerRepository.GetAsync(searchText);
        }

        public async Task<Officer> GetOfficerLoginAsync(string identificationNumber, string password)
        {
            return await _officerRepository.GetOfficerLoginAsync(identificationNumber, password);
        }

        public async Task<string> AddOfficerAsync(string param)
        {
            return await _officerRepository.AddAsync(param);
        }

        public async Task<string> UpdateOfficerAsync(string param)
        {
            return await _officerRepository.UpdateAsync(param);
        }

        public async Task<string> DeleteOfficerAsync(string param)
        {
            return await _officerRepository.DeleteAsync(param);
        }

    }
}
