using DNATestingSystem.Repository.TienDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public interface IAppointmentsTienDmService
    {
        Task<List<AppointmentsTienDm>> GetAllAsyn();
        Task<AppointmentsTienDm> GetByIdAsync(int id);
        Task<List<AppointmentsTienDm>> SearchAsync(int id, string contactPhone, decimal totalAmount);
        Task<int> CreateAsync(AppointmentsTienDm entity);
        Task<int> UpdateAsync(AppointmentsTienDm entity);
        Task<bool> DeleteAsync(int id);

    }
}
