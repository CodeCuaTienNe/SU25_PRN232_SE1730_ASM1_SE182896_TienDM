using DNATestingSystem.Repository.TienDM;
using DNATestingSystem.Repository.TienDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public class AppointmentsTienDmService : IAppointmentsTienDmService
    {
        private readonly AppointmentsTienDmRepository _repository;
        public AppointmentsTienDmService()
            => _repository = new AppointmentsTienDmRepository();

        public async Task<int> CreateAsync(AppointmentsTienDm appointmentsTien)
        {
            return await _repository.CreateAsync(appointmentsTien);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<List<AppointmentsTienDm>> GetAllAsyn()
        {
            return await _repository.GetAllAsync();

        }

        public async Task<AppointmentsTienDm> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);

        }

        public async Task<List<AppointmentsTienDm>> SearchAsync(int id, string contactPhone, decimal totalAmount)
        {
            return await _repository.SearchAsync(id, contactPhone, totalAmount);
        }

        public async Task<int> UpdateAsync(AppointmentsTienDm appointmentsTien)
        {
            return await _repository.UpdateAsync(appointmentsTien);
        }
    }
}
