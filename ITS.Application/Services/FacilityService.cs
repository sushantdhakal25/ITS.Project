using ITS.Application.Interfaces;
using ITS.Domain.Repositories;

namespace ITS.Application.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly IFacilityRepository _facilityRepository;

        public FacilityService(IFacilityRepository facilityRepository)
        {
            _facilityRepository = facilityRepository;
        }

        public async Task<string> GetFacilitiesAsync(string? searchText)
        {
            return await _facilityRepository.GetAsync(searchText);
        }

        public async Task<string> AddFacilityAsync(string param)
        {
            return await _facilityRepository.AddAsync(param);
        }

        public async Task<string> UpdateFacilityAsync(string param)
        {
            return await _facilityRepository.UpdateAsync(param);
        }

        public async Task<string> DeleteFacilityAsync(string param)
        {
            return await _facilityRepository.DeleteAsync(param);
        }
    }
}
