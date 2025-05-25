using DNATestingSystem.Repository.TienDM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public interface IUserAccountService
    {
        Task<UserAccount?> GetUserAccount(string userName, string password);
        Task<UserAccount?> GetUserAccountById(int userId);
    }
}
