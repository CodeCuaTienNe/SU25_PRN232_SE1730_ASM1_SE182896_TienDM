using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Repository.TienDM.ModelExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public interface IAppointmentsTienDmService
    {
        Task<List<AppointmentsTienDmDto>> GetAllAsync();
        Task<AppointmentsTienDmDto?> GetByIdAsync(int id);
        Task<PaginationResult<List<AppointmentsTienDmDto>>> SearchAsync(SearchAppointmentsTienDm searchRequest);
        Task<int> CreateAsync(AppointmentsTienDmCreateRequest request, int? userId = null);
        Task<int> UpdateAsync(int id, AppointmentsTienDmCreateRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
