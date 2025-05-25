using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Repository.TienDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public class UserAccountService : IUserAccountService
    {
        private readonly UserAccountRepository _repository;
        public UserAccountService()
            => _repository = new UserAccountRepository();
        
        public Task<UserAccount?> GetUserAccount(string userName, string password)
        {
            return _repository.GetUserAccount(userName, password);
        }

        public Task<UserAccount?> GetUserAccountById(int userId)
        {
            return _repository.GetUserAccountById(userId);
        }
    }
}
