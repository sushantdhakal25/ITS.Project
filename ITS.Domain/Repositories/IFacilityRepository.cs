using ITS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITS.Domain.Repositories
{
    public interface IFacilityRepository
    {
        Task<string> GetAsync(string? searchText);
        Task<string> AddAsync(string param);
        Task<string> UpdateAsync(string param);
        Task<string> DeleteAsync(string param);
    }
}
