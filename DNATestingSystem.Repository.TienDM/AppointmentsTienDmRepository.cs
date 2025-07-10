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
        public AppointmentsTienDmRepository() : base() { }

        public AppointmentsTienDmRepository(SE18_PRN232_SE1730_G3_DNATestingSystemContext context) : base(context) { }
        public new async Task<List<AppointmentsTienDm>> GetAllAsync()
        {
            try
            {
                var appointments = await _context.AppointmentsTienDms
                    .ToListAsync();

                return appointments ?? new List<AppointmentsTienDm>();
            }
            catch (Exception ex)
            {
                // Log the exception or rethrow with more details
                throw new Exception($"Error in GetAllAsync: {ex.Message}", ex);
            }
        }

        public new async Task<AppointmentsTienDm> GetByIdAsync(int id)
        {
            try
            {
                // Try without includes first to isolate the issue
                var appointment = await _context.AppointmentsTienDms
                    .FirstOrDefaultAsync(a => a.AppointmentsTienDmid == id);

                return appointment ?? new AppointmentsTienDm();
            }
            catch (Exception ex)
            {
                // Log the exception or rethrow with more details
                throw new Exception($"Error in GetByIdAsync: {ex.Message}", ex);
            }
        }


        public async Task<PaginationResult<List<AppointmentsTienDm>>> GetAllPaginatedAsync(int page, int pageSize)
        {
            // Use empty search criteria to get all items with pagination
            return await SearchAsync(0, string.Empty, 0, page, pageSize);
        }


        public async Task<PaginationResult<List<AppointmentsTienDm>>> SearchAsync(int id, string contactPhone, decimal totalAmount, int page, int pageSize)
        {
            var query = BuildSearchQuery(id, contactPhone, totalAmount);
            return await ExecutePaginatedQuery(query, page, pageSize);
        }


        public async Task<PaginationResult<List<AppointmentsTienDm>>> SearchAsync(SearchAppointmentsTienDm searchRequest)
        {
            // Set default values if null
            var page = searchRequest.CurrentPage ?? 1;
            var pageSize = searchRequest.PageSize ?? 10;
            var contactPhone = searchRequest.ContactPhone;
            var totalAmount = searchRequest.TotalAmount ?? 0;
            var id = searchRequest.AppointmentsTienDmid ?? 0;

            var query = BuildSearchQuery(id, contactPhone, totalAmount);
            return await ExecutePaginatedQuery(query, page, pageSize);
        }

        private IQueryable<AppointmentsTienDm> BuildSearchQuery(int id, string? contactPhone, decimal totalAmount)
        {
            // Include navigation properties for better data display
            return _context.AppointmentsTienDms
                .Include(a => a.ServicesNhanVt) // for ServiceName
                .Include(a => a.UserAccount)    // for Username
                .Include(a => a.AppointmentStatusesTienDm) // for StatusName
                .AsNoTracking() // ensure no tracking for read-only queries
                .Where(a => (string.IsNullOrEmpty(contactPhone) || a.ContactPhone.Contains(contactPhone))
                    && (totalAmount == 0 || a.TotalAmount == totalAmount)
                    && (id == 0 || a.AppointmentsTienDmid == id));
        }

        private async Task<PaginationResult<List<AppointmentsTienDm>>> ExecutePaginatedQuery(IQueryable<AppointmentsTienDm> query, int page, int pageSize)
        {
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

            return await base.RemoveAsync(appointment);
        }
    }
}
