using DNATestingSystem.Repository.TienDM.Basic;
using DNATestingSystem.Repository.TienDM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Repository.TienDM
{
    public class UserAccountRepository : GenericRepository<UserAccount>
    {
        public UserAccountRepository() { }
        public UserAccountRepository(SE18_PRN232_SE1730_G3_DNATestingSystemContext context)
        => _context = context;        public async Task<UserAccount?> GetUserAccount(string userName, string password)
        {
            return await _context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
        }

        public async Task<UserAccount?> GetUserAccountById(int userId)
        {
            return await _context.UserAccounts.FirstOrDefaultAsync(u => u.UserAccountId == userId);
        }
    }
}
