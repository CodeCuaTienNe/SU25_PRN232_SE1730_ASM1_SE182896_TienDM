using DNATestingSystem.Repository.TienDM;
using DNATestingSystem.Repository.TienDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public class ServicesNhanVtService : IServicesNhanVtService
    {
        private readonly ServicesNhanVtRepository _repository;

        public ServicesNhanVtService()
            => _repository = new ServicesNhanVtRepository();

        public async Task<List<ServicesNhanVt>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<ServicesNhanVt> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<List<ServicesNhanVt>> GetActiveServicesAsync()
        {
            return await _repository.GetActiveServicesAsync();
        }

        public async Task<List<ServicesNhanVt>> SearchAsync(int id, string serviceName)
        {
            return await _repository.SearchAsync(id, serviceName ?? "");
        }

        public async Task<int> CreateAsync(ServicesNhanVt entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<int> UpdateAsync(ServicesNhanVt entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
