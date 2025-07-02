using DNATestingSystem.Repository.TienDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public interface IServicesNhanVtService
    {
        Task<List<ServicesNhanVt>> GetAllAsync();
        Task<ServicesNhanVt> GetByIdAsync(int id);
        Task<List<ServicesNhanVt>> GetActiveServicesAsync();
        Task<List<ServicesNhanVt>> SearchAsync(int id, string serviceName);
        Task<int> CreateAsync(ServicesNhanVt entity);
        Task<int> UpdateAsync(ServicesNhanVt entity);
        Task<bool> DeleteAsync(int id);
    }
}
