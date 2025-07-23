using DNATestingSystem.Repository.TienDM;
using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Repository.TienDM.ModelExtensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    public class AppointmentsTienDmService : IAppointmentsTienDmService
    {
        private readonly AppointmentsTienDmRepository _repository;
        public AppointmentsTienDmService()
        {
            _repository = new AppointmentsTienDmRepository();
        }
        public async Task<List<AppointmentsTienDm>> GetAllBasicAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<List<AppointmentsTienDmDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            // Always map to DTO and never expose entity directly
            return entities.Select(MapToDto).ToList();
        }

        public async Task<AppointmentsTienDmDto?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null || entity.AppointmentsTienDmid == 0) return null;
            return MapToDto(entity);
        }

        public async Task<PaginationResult<List<AppointmentsTienDmDto>>> SearchAsync(SearchAppointmentsTienDm searchRequest)
        {
            var result = await _repository.SearchAsync(searchRequest);
            return new PaginationResult<List<AppointmentsTienDmDto>>
            {
                TotalItems = result.TotalItems,
                TotalPages = result.TotalPages,
                CurrentPages = result.CurrentPages,
                PageSize = result.PageSize,
                Items = result.Items.Select(MapToDto).ToList()
            };
        }

        public async Task<int> CreateAsync(AppointmentsTienDmCreateRequest request, int? userId = null)
        {
            var entity = new AppointmentsTienDm
            {
                UserAccountId = userId ?? request.UserAccountId ?? 0,
                ServicesNhanVtid = request.ServicesNhanVtid ?? 0,
                AppointmentStatusesTienDmid = request.AppointmentStatusesTienDmid ?? 0,
                AppointmentDate = request.AppointmentDate.HasValue ? DateOnly.FromDateTime(request.AppointmentDate.Value) : DateOnly.FromDateTime(DateTime.Now),
                AppointmentTime = !string.IsNullOrEmpty(request.AppointmentTime) ? TimeOnly.Parse(request.AppointmentTime) : new TimeOnly(0, 0),
                SamplingMethod = request.SamplingMethod ?? string.Empty,
                Address = request.Address,
                ContactPhone = request.ContactPhone ?? string.Empty,
                Notes = request.Notes,
                TotalAmount = request.TotalAmount ?? 0,
                IsPaid = request.IsPaid,
                AppointmentStatusesTienDm = null,
                ServicesNhanVt = null,
                UserAccount = null
            };
            return await _repository.CreateAsync(entity);
        }

        public async Task<int> UpdateAsync(int id, AppointmentsTienDmCreateRequest request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return 0;
            entity.UserAccountId = request.UserAccountId ?? entity.UserAccountId;
            entity.ServicesNhanVtid = request.ServicesNhanVtid ?? entity.ServicesNhanVtid;
            entity.AppointmentStatusesTienDmid = request.AppointmentStatusesTienDmid ?? entity.AppointmentStatusesTienDmid;
            if (request.AppointmentDate.HasValue)
                entity.AppointmentDate = DateOnly.FromDateTime(request.AppointmentDate.Value);
            if (!string.IsNullOrEmpty(request.AppointmentTime))
                entity.AppointmentTime = TimeOnly.Parse(request.AppointmentTime);
            entity.SamplingMethod = request.SamplingMethod ?? entity.SamplingMethod;
            entity.Address = request.Address ?? entity.Address;
            entity.ContactPhone = request.ContactPhone ?? entity.ContactPhone;
            entity.Notes = request.Notes ?? entity.Notes;
            entity.TotalAmount = request.TotalAmount ?? entity.TotalAmount;
            entity.IsPaid = request.IsPaid ?? entity.IsPaid;
            entity.AppointmentStatusesTienDm = null;
            entity.ServicesNhanVt = null;
            entity.UserAccount = null;
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private AppointmentsTienDmDto MapToDto(AppointmentsTienDm entity)
        {
            return new AppointmentsTienDmDto
            {
                AppointmentsTienDmid = entity.AppointmentsTienDmid,
                UserAccountId = entity.UserAccountId,
                Username = entity.UserAccount?.UserName,
                ServicesNhanVtid = entity.ServicesNhanVtid,
                ServiceName = entity.ServicesNhanVt?.ServiceName,
                AppointmentStatusesTienDmid = entity.AppointmentStatusesTienDmid,
                StatusName = entity.AppointmentStatusesTienDm?.StatusName,
                AppointmentDate = entity.AppointmentDate,
                AppointmentTime = entity.AppointmentTime,
                SamplingMethod = entity.SamplingMethod,
                Address = entity.Address,
                ContactPhone = entity.ContactPhone,
                Notes = entity.Notes,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate,
                TotalAmount = entity.TotalAmount,
                IsPaid = entity.IsPaid
            };
        }
    }
}
