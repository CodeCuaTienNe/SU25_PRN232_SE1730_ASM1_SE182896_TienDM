using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Services.TienDM;
using HotChocolate;

namespace DNATestingSystem.GraphQLAPIServices.TienDM.GraphQLs
{
    public class Mutations
    {
        private readonly IServiceProviders _serviceProviders;

        public Mutations(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        #region Mutation for AppointmentsTienDm

        //Qua nhieu truong khoa ngoai, can DTO + JSON ignore, về làm lại
        public async Task<int> CreateAppointmentsTienDm(AppointmentsTienDm appointmentsTienDm)
        {
            try
            {
                var result = await _serviceProviders.AppointmentsTienDmService.CreateAsync(appointmentsTienDm);
                return result;
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                throw new GraphQLException($"Failed to create appointment: {ex.Message}");
            }
        }

        public async Task<int> UpdateAppointmentsTienDm(AppointmentsTienDm appointmentsTienDm)
        {
            try
            {
                var result = await _serviceProviders.AppointmentsTienDmService.UpdateAsync(appointmentsTienDm);
                return result;
            }
            catch (Exception ex) { }
            return 0;
        }
        public async Task<int> DeleteAppointmentsTienDm(int id)
        {
            try
            {
                var result = await _serviceProviders.AppointmentsTienDmService.DeleteAsync(id);
                return result ? 1 : 0;
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                throw new GraphQLException($"Failed to delete appointment: {ex.Message}");
            }
        }
        #endregion

        #region Mutation for AppointmentStatusesTienDm
        public async Task<int> CreateAppointmentStatusesTienDm(AppointmentStatusesTienDm appointmentStatus)
        {
            try
            {
                var result = await _serviceProviders.AppointmentStatusesTienDmService.CreateAsync(appointmentStatus);
                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to create appointment status: {ex.Message}");
            }
        }

        public async Task<int> UpdateAppointmentStatusesTienDm(AppointmentStatusesTienDm appointmentStatus)
        {
            try
            {
                var result = await _serviceProviders.AppointmentStatusesTienDmService.UpdateAsync(appointmentStatus);
                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to update appointment status: {ex.Message}");
            }
        }

        public async Task<int> DeleteAppointmentStatusesTienDm(int id)
        {
            try
            {
                var result = await _serviceProviders.AppointmentStatusesTienDmService.DeleteAsync(id);
                return result ? 1 : 0;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to delete appointment status: {ex.Message}");
            }
        }
        #endregion

        #region Mutation for ServicesNhanVt
        public async Task<int> CreateServicesNhanVt(ServicesNhanVt service)
        {
            try
            {
                var result = await _serviceProviders.ServicesNhanVtService.CreateAsync(service);
                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to create service: {ex.Message}");
            }
        }

        public async Task<int> UpdateServicesNhanVt(ServicesNhanVt service)
        {
            try
            {
                var result = await _serviceProviders.ServicesNhanVtService.UpdateAsync(service);
                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to update service: {ex.Message}");
            }
        }

        public async Task<int> DeleteServicesNhanVt(int id)
        {
            try
            {
                var result = await _serviceProviders.ServicesNhanVtService.DeleteAsync(id);
                return result ? 1 : 0;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to delete service: {ex.Message}");
            }
        }
        #endregion

        #region Mutation for SystemUserAccount
        public async Task<int> CreateSystemUserAccount(SystemUserAccount userAccount)
        {
            try
            {
                var result = await _serviceProviders.SystemUserAccountService.CreateAsync(userAccount);
                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to create user account: {ex.Message}");
            }
        }

        public async Task<int> UpdateSystemUserAccount(SystemUserAccount userAccount)
        {
            try
            {
                var result = await _serviceProviders.SystemUserAccountService.UpdateAsync(userAccount);
                return result;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to update user account: {ex.Message}");
            }
        }

        public async Task<int> DeleteSystemUserAccount(int id)
        {
            try
            {
                var result = await _serviceProviders.SystemUserAccountService.DeleteAsync(id);
                return result ? 1 : 0;
            }
            catch (Exception ex)
            {
                throw new GraphQLException($"Failed to delete user account: {ex.Message}");
            }
        }
        #endregion
    }
}
