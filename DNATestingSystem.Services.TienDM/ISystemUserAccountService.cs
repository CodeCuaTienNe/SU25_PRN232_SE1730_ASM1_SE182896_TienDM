using DNATestingSystem.Repository.TienDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public interface ISystemUserAccountService
    {
        Task<SystemUserAccount?> GetUserAccount(string userName, string password);
        Task<SystemUserAccount?> GetUserAccountById(int userId);
        Task<List<SystemUserAccount>> GetAllAsync();
        Task<int> CreateAsync(SystemUserAccount userAccount);
        Task<int> UpdateAsync(SystemUserAccount userAccount);
        Task<bool> DeleteAsync(int id);
    }
}
