using DNATestingSystem.Repository.TienDM.Basic;
using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Repository.TienDM.ModelExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNATestingSystem.Repository.TienDM.DBContext;


namespace DNATestingSystem.Repository.TienDM
{
    public class AppointmentsTienDmRepository : GenericRepository<AppointmentsTienDm>
    {
        public AppointmentsTienDmRepository() { }
        public AppointmentsTienDmRepository(SE18_PRN232_SE1730_G3_DNATestingSystemContext context) => _context = context; public new async Task<List<AppointmentsTienDm>> GetAllAsync()
        {
            var appointments = await _context.AppointmentsTienDms
                .Include(a => a.AppointmentStatusesTienDm)
                .Include(a => a.ServicesNhanVt)
                .Include(a => a.UserAccount)
                .ToListAsync();
            return appointments ?? new List<AppointmentsTienDm>();
        }
        public new async Task<AppointmentsTienDm> GetByIdAsync(int id)
        {
            var appointment = await _context.AppointmentsTienDms
                .Include(a => a.AppointmentStatusesTienDm)
                .Include(a => a.ServicesNhanVt)
                .Include(a => a.UserAccount)
                .FirstOrDefaultAsync(a => a.AppointmentsTienDmid == id);
            return appointment ?? new AppointmentsTienDm();
        }
        public async Task<PaginationResult<List<AppointmentsTienDm>>> SearchAsync(int id, string contactPhone, decimal totalAmount, int page, int pageSize)
        {
            // Build the query without executing it
            var query = _context.AppointmentsTienDms
                .Include(a => a.AppointmentStatusesTienDm)
                .Include(a => a.ServicesNhanVt)
                .Include(a => a.UserAccount)
                .Where(a => (string.IsNullOrEmpty(contactPhone) || a.ContactPhone.Contains(contactPhone))
                    && (totalAmount == 0 || a.TotalAmount == totalAmount)
                    && (id == 0 || a.AppointmentsTienDmid == id));

            // Get total count for pagination
            var totalItems = await query.CountAsync();

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            // Apply pagination
            var appointments = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResult<List<AppointmentsTienDm>>
            {
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPages = page,
                PageSize = pageSize,
                Items = appointments ?? new List<AppointmentsTienDm>()
            };
        }
        public new async Task<int> CreateAsync(AppointmentsTienDm entity)
        {
            if (entity.CreatedDate == null)
                entity.CreatedDate = DateTime.Now;

            return await base.CreateAsync(entity);
        }

        public new async Task<int> UpdateAsync(AppointmentsTienDm entity)
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
