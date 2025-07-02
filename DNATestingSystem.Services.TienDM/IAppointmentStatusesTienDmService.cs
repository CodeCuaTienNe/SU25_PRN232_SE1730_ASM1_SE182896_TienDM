using DNATestingSystem.Repository.TienDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public interface IAppointmentStatusesTienDmService
    {
        Task<List<AppointmentStatusesTienDm>> GetAllAsync();
        Task<AppointmentStatusesTienDm> GetByIdAsync(int id);
        Task<List<AppointmentStatusesTienDm>> GetActiveStatusesAsync();
        Task<List<AppointmentStatusesTienDm>> SearchAsync(int id, string statusName);
        Task<int> CreateAsync(AppointmentStatusesTienDm entity);
        Task<int> UpdateAsync(AppointmentStatusesTienDm entity);
        Task<bool> DeleteAsync(int id);
    }
}
