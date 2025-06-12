using DNATestingSystem.Repository.TienDM;
using DNATestingSystem.Repository.TienDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public class AppointmentStatusesTienDmService : IAppointmentStatusesTienDmService
    {
        private readonly AppointmentStatusesTienDmRepository _repository;

        public AppointmentStatusesTienDmService()
            => _repository = new AppointmentStatusesTienDmRepository();

        public async Task<List<AppointmentStatusesTienDm>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<AppointmentStatusesTienDm> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<AppointmentStatusesTienDm>> GetActiveStatusesAsync()
        {
            return await _repository.GetActiveStatusesAsync();
        }

        public async Task<List<AppointmentStatusesTienDm>> SearchAsync(int id, string statusName)
        {
            return await _repository.SearchAsync(id, statusName ?? "");
        }

        public async Task<int> CreateAsync(AppointmentStatusesTienDm entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<int> UpdateAsync(AppointmentStatusesTienDm entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
