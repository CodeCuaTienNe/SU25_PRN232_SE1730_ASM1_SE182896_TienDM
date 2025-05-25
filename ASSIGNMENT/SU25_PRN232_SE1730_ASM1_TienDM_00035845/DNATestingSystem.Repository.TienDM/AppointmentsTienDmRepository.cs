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
    public class AppointmentsTienDmRepository : GenericRepository<AppointmentsTienDm>
    {
        public AppointmentsTienDmRepository() { }
        public AppointmentsTienDmRepository(SE18_PRN232_SE1730_G3_DNATestingSystemContext context) => _context = context;

        public new async Task<List<AppointmentsTienDm>> GetAllAsync()
        {
            var appointments = await _context.AppointmentsTienDms
                .Include(a => a.AppointmentStatusesTienDm)
                .Include(a => a.ServicesNhanVt)
                .Include(a => a.UserAccount)
                .ToListAsync();
            return appointments ?? new List<AppointmentsTienDm>();
        }

        public async Task<AppointmentsTienDm> GetByIdAsync(int id)
        {
            var appointment = await _context.AppointmentsTienDms
                .Include(a => a.AppointmentStatusesTienDm)
                .Include(a => a.ServicesNhanVt)
                .Include(a => a.UserAccount)
                .Include(a => a.SampleThinhLcs)
                .FirstOrDefaultAsync(a => a.AppointmentsTienDmid == id);
            return appointment ?? new AppointmentsTienDm();
        }

        public async Task<List<AppointmentsTienDm>> SearchAsync(int id, string contactPhone, decimal totalAmount)
        {
            var appointments = await _context.AppointmentsTienDms
                .Include(a => a.AppointmentStatusesTienDm)
                .Include(a => a.ServicesNhanVt)
                .Include(a => a.UserAccount)
                .Where(a => (a.ContactPhone.Contains(contactPhone) || string.IsNullOrEmpty(contactPhone))
                    && (a.TotalAmount == totalAmount || totalAmount == 0)
                    && (a.AppointmentsTienDmid == id || id == 0))
                .ToListAsync();
            return appointments ?? new List<AppointmentsTienDm>();
        }

        public async Task<int> CreateAsync(AppointmentsTienDm entity)
        {
            if (entity.CreatedDate == null)
                entity.CreatedDate = DateTime.Now;

            return await base.CreateAsync(entity);
        }

        public async Task<int> UpdateAsync(AppointmentsTienDm entity)
        {
            entity.ModifiedDate = DateTime.Now;
            return await base.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var appointment = await _context.AppointmentsTienDms.FindAsync(id);
            if (appointment == null)
                return false;

            return await RemoveAsync(appointment);
        }
    }
}
