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
                var appointments = await _context.AppointmentsTienDms.Include(a => a.ServicesNhanVt).Include(b => b.AppointmentStatusesTienDm).OrderByDescending(o => o.AppointmentsTienDmid)
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
            // Use empty search criteria to get all items with pagination, sorted by id desc
            var result = await SearchAsync(0, string.Empty, 0, page, pageSize);
            if (result == null)
            {
                return new PaginationResult<List<AppointmentsTienDm>>
                {
                    TotalItems = 0,
                    TotalPages = 0,
                    CurrentPages = page,
                    PageSize = pageSize,
                    Items = new List<AppointmentsTienDm>()
                };
            }
            if (result.Items != null)
            {
                result.Items = result.Items.OrderByDescending(a => a.AppointmentsTienDmid).ToList();
            }
            else
            {
                result.Items = new List<AppointmentsTienDm>();
            }
            return result;
        }


        public async Task<PaginationResult<List<AppointmentsTienDm>>> SearchAsync(int id, string contactPhone, decimal totalAmount, int page, int pageSize)
        {
            var query = BuildSearchQuery(id, contactPhone, totalAmount).OrderByDescending(a => a.AppointmentsTienDmid);
            return await ExecutePaginatedQuery(query, page, pageSize);
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
        public async Task<PaginationResult<List<AppointmentsTienDm>>> SearchAsync(SearchAppointmentsTienDm searchRequest)
        {
            // Set default values if null
            var page = searchRequest.CurrentPage ?? 1;
            var pageSize = searchRequest.PageSize ?? 10;
            var contactPhone = searchRequest.ContactPhone;
            var totalAmount = searchRequest.TotalAmount ?? 0;
            var id = searchRequest.AppointmentsTienDmid ?? 0;

            var query = BuildSearchQuery(id, contactPhone, totalAmount).OrderByDescending(a => a.AppointmentsTienDmid);
            return await ExecutePaginatedQuery(query, page, pageSize);
        }

        private IQueryable<AppointmentsTienDm> BuildSearchQuery(int id, string? contactPhone, decimal totalAmount)
        {
            return _context.AppointmentsTienDms
                .Include(a => a.ServicesNhanVt)
                .Include(a => a.UserAccount)
                .Include(a => a.AppointmentStatusesTienDm)
                .AsNoTracking() // ensure no tracking for read-only queries
                .Where(a => (string.IsNullOrEmpty(contactPhone) || a.ContactPhone.Contains(contactPhone))
                    && (totalAmount == 0 || a.TotalAmount == totalAmount)
                    && (id == 0 || a.AppointmentsTienDmid == id));
        }

        private async Task<PaginationResult<List<AppointmentsTienDm>>> ExecutePaginatedQuery(IQueryable<AppointmentsTienDm> query, int page, int pageSize)
        {
            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            // Ensure sorting by id desc for pagination as well
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
    }
}
