using AutoMapper;
using ITS.Application.DTOs;
using ITS.Application.Interfaces;
using ITS.Domain.Entities;
using ITS.Domain.Repositories;

namespace ITS.Application.Services
{
    public class OfficerService : IOfficerService
    {
        private readonly IOfficerRepository _officerRepository;
        private readonly IMapper _mapper;

        public OfficerService(IOfficerRepository officerRepository, IMapper mapper)
        {
            _officerRepository = officerRepository;
            _mapper = mapper;
        }

        public async Task<string> GetOfficersAsync(string? searchText)
        {
            return await _officerRepository.GetAsync(searchText);
        }

        public async Task<OfficerDto> GetOfficerLoginAsync(string identificationNumber, string password)
        {
            var result = await _officerRepository.GetOfficerLoginAsync(identificationNumber, password);
            return _mapper.Map<OfficerDto>(result);
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
