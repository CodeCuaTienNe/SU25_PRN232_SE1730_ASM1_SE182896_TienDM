using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Repository.TienDM.ModelExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public interface IAppointmentsTienDmService
    {
        Task<List<AppointmentsTienDm>> GetAllAsync();
        Task<PaginationResult<List<AppointmentsTienDm>>> GetAllPaginatedAsync(int page, int pageSize);
        Task<AppointmentsTienDm> GetByIdAsync(int id);
        Task<PaginationResult<List<AppointmentsTienDm>>> SearchAsync(int id, string contactPhone, decimal totalAmount, int page, int pageSize);
        Task<PaginationResult<List<AppointmentsTienDm>>> SearchAsync(SearchAppointmentsTienDm searchRequest);
        Task<int> CreateAsync(AppointmentsTienDm entity);
        Task<int> UpdateAsync(AppointmentsTienDm entity);
        Task<bool> DeleteAsync(int id);
    }
}
