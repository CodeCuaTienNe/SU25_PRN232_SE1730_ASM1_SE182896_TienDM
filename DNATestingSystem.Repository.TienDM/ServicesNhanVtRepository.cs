using DNATestingSystem.Repository.TienDM.Basic;
using DNATestingSystem.Repository.TienDM.DBContext;
using DNATestingSystem.Repository.TienDM.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNATestingSystem.Repository.TienDM
{
    public class ServicesNhanVtRepository : GenericRepository<ServicesNhanVt>
    {
        public ServicesNhanVtRepository() { }
        public ServicesNhanVtRepository(SE18_PRN232_SE1730_G3_DNATestingSystemContext context) => _context = context;

        public new async Task<List<ServicesNhanVt>> GetAllAsync()
        {
            var services = await _context.ServicesNhanVts
                .Include(s => s.ServiceCategoryNhanVt)
                .Include(s => s.UserAccount)
                .Where(s => s.IsActive == true)
                .ToListAsync();
            return services ?? new List<ServicesNhanVt>();
        }

        public new async Task<ServicesNhanVt> GetByIdAsync(int id)
        {
            var service = await _context.ServicesNhanVts
                .Include(s => s.ServiceCategoryNhanVt)
                .Include(s => s.UserAccount)
                .Include(s => s.AppointmentsTienDms)
                .FirstOrDefaultAsync(s => s.ServicesNhanVtid == id);
            return service ?? new ServicesNhanVt();
        }

        public async Task<List<ServicesNhanVt>> GetActiveServicesAsync()
        {
            var services = await _context.ServicesNhanVts
                .Include(s => s.ServiceCategoryNhanVt)
                .Include(s => s.UserAccount)
                .Where(s => s.IsActive == true)
                .ToListAsync();
            return services ?? new List<ServicesNhanVt>();
        }

        public async Task<List<ServicesNhanVt>> SearchAsync(int id, string serviceName)
        {
            var services = await _context.ServicesNhanVts
                .Include(s => s.ServiceCategoryNhanVt)
                .Include(s => s.UserAccount)
                .Where(s => (s.ServiceName.Contains(serviceName) || string.IsNullOrEmpty(serviceName))
                    && (s.ServicesNhanVtid == id || id == 0))
                .ToListAsync();
            return services ?? new List<ServicesNhanVt>();
        }

        public new async Task<int> CreateAsync(ServicesNhanVt entity)
        {
            if (entity.CreatedDate == null)
                entity.CreatedDate = DateTime.Now;

            if (entity.IsActive == null)
                entity.IsActive = true;

            return await base.CreateAsync(entity);
        }

        public new async Task<int> UpdateAsync(ServicesNhanVt entity)
        {
            entity.ModifiedDate = DateTime.Now;
            return await base.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var service = await _context.ServicesNhanVts.FindAsync(id);
            if (service == null)
                return false;

            return await RemoveAsync(service);
        }
    }
}
