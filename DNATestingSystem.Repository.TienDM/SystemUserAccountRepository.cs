using DNATestingSystem.Repository.TienDM.Basic;
using DNATestingSystem.Repository.TienDM.DBContext;
using DNATestingSystem.Repository.TienDM.Models;
using Microsoft.EntityFrameworkCore;

namespace DNATestingSystem.Repository.TienDM
{
    public class UserAccountRepository : GenericRepository<SystemUserAccount>
    {
        public UserAccountRepository() { }
        public UserAccountRepository(SE18_PRN232_SE1730_G3_DNATestingSystemContext context)
        => _context = context;       
        public async Task<SystemUserAccount?> GetUserAccount(string userName, string password)
        {
            return await _context.SystemUserAccounts.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password);
        }        public async Task<SystemUserAccount?> GetUserAccountById(int userId)
        {
            return await _context.SystemUserAccounts.FirstOrDefaultAsync(u => u.UserAccountId == userId);
        }

        // Create UserAccount
        public new async Task<int> CreateAsync(SystemUserAccount userAccount)
        {
            if (userAccount.CreatedDate == null)
                userAccount.CreatedDate = DateTime.Now;
            
            return await base.CreateAsync(userAccount);
        }

        // Update UserAccount
        public new async Task<int> UpdateAsync(SystemUserAccount userAccount)
        {
            userAccount.ModifiedDate = DateTime.Now;
            return await base.UpdateAsync(userAccount);
        }

        // Delete UserAccount
        public async Task<bool> DeleteAsync(int id)
        {
            var userAccount = await _context.SystemUserAccounts.FindAsync(id);
            if (userAccount == null)
                return false;

            return await RemoveAsync(userAccount);
        }

        // Get all UserAccounts
        public new async Task<List<SystemUserAccount>> GetAllAsync()
        {
            var users = await _context.SystemUserAccounts.ToListAsync();
            return users ?? new List<SystemUserAccount>();
        }
    }
}
