using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Repository.TienDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{    public class SystemUserAccountService : ISystemUserAccountService
    {
        private readonly UserAccountRepository _repository;
        public SystemUserAccountService()
            => _repository = new UserAccountRepository();
        
        public Task<SystemUserAccount?> GetUserAccount(string userName, string password)
        {
            return _repository.GetUserAccount(userName, password);
        }

        public Task<SystemUserAccount?> GetUserAccountById(int userId)
        {
            return _repository.GetUserAccountById(userId);
        }

        public async Task<List<SystemUserAccount>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<int> CreateAsync(SystemUserAccount userAccount)
        {
            return await _repository.CreateAsync(userAccount);
        }

        public async Task<int> UpdateAsync(SystemUserAccount userAccount)
        {
            return await _repository.UpdateAsync(userAccount);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
