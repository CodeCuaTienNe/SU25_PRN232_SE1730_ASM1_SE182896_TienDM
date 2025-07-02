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
    public class AppointmentStatusesTienDmRepository : GenericRepository<AppointmentStatusesTienDm>
    {
        public AppointmentStatusesTienDmRepository() { }
        public AppointmentStatusesTienDmRepository(SE18_PRN232_SE1730_G3_DNATestingSystemContext context) => _context = context;

        public new async Task<List<AppointmentStatusesTienDm>> GetAllAsync()
        {
            var statuses = await _context.AppointmentStatusesTienDms
                .Include(s => s.AppointmentsTienDms)
                .ToListAsync();
            return statuses ?? new List<AppointmentStatusesTienDm>();
        }

        public new async Task<AppointmentStatusesTienDm> GetByIdAsync(int id)
        {
            var status = await _context.AppointmentStatusesTienDms
                .Include(s => s.AppointmentsTienDms)
                .FirstOrDefaultAsync(s => s.AppointmentStatusesTienDmid == id);
            return status ?? new AppointmentStatusesTienDm();
        }

        public async Task<List<AppointmentStatusesTienDm>> GetActiveStatusesAsync()
        {
            var statuses = await _context.AppointmentStatusesTienDms
                .Where(s => s.IsActive == true)
                .ToListAsync();
            return statuses ?? new List<AppointmentStatusesTienDm>();
        }

        public async Task<List<AppointmentStatusesTienDm>> SearchAsync(int id, string statusName)
        {
            var statuses = await _context.AppointmentStatusesTienDms
                .Where(s => (s.StatusName.Contains(statusName) || string.IsNullOrEmpty(statusName))
                    && (s.AppointmentStatusesTienDmid == id || id == 0))
                .ToListAsync();
            return statuses ?? new List<AppointmentStatusesTienDm>();
        }

        public new async Task<int> CreateAsync(AppointmentStatusesTienDm entity)
        {
            if (entity.CreatedDate == null)
                entity.CreatedDate = DateTime.Now;

            if (entity.IsActive == null)
                entity.IsActive = true;

            return await base.CreateAsync(entity);
        }

        public new async Task<int> UpdateAsync(AppointmentStatusesTienDm entity)
        {
            return await base.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var status = await _context.AppointmentStatusesTienDms.FindAsync(id);
            if (status == null)
                return false;

            // Check if status is being used in appointments
            var hasAppointments = await _context.AppointmentsTienDms
                .AnyAsync(a => a.AppointmentStatusesTienDmid == id);

            if (hasAppointments)
            {
                // Soft delete - just mark as inactive
                status.IsActive = false;
                await base.UpdateAsync(status);
                return true;
            }
            else
            {
                // Hard delete if not being used
                return await RemoveAsync(status);
            }
        }
    }
}
