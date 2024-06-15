using ITS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.Application.Interfaces
{
    public interface IFacilityService
    {
        //Task<IEnumerable<Facility>> GetAllFacilitiesAsync();

        Task<string> GetFacilitiesAsync(string? searchText);
        Task<string> AddFacilityAsync(string param);
        Task<string> UpdateFacilityAsync(string param);
        Task<string> DeleteFacilityAsync(string param);
    }
}
