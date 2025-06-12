using DNATestingSystem.Repository.TienDM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    /// <summary>
    /// Example controller or business logic class demonstrating how to use ServiceProviders pattern
    /// This shows how to access multiple services through a single provider
    /// </summary>
    public class ExampleServiceProvidersUsage
    {
        private readonly IServiceProviders _serviceProviders;

        public ExampleServiceProvidersUsage(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        /// <summary>
        /// Example: User authentication with service providers
        /// </summary>
        public async Task<SystemUserAccount?> AuthenticateUserAsync(string userName, string password)
        {
            try
            {
                // Use SystemUserAccountService through ServiceProviders
                var user = await _serviceProviders.SystemUserAccountService.GetUserAccount(userName, password);

                if (user != null)
                {
                    // Could also update user login timestamp using the same service
                    user.ModifiedDate = DateTime.Now;
                    await _serviceProviders.SystemUserAccountService.UpdateAsync(user);
                }

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to authenticate user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Example: Get user's appointments with related services information
        /// </summary>
        public async Task<List<AppointmentsTienDm>> GetUserAppointmentsWithServicesAsync(int userId)
        {
            try
            {
                // Use multiple services through ServiceProviders
                var appointments = await _serviceProviders.AppointmentsTienDmService.GetAllAsync();

                // Filter by user (this could be moved to repository level)
                var userAppointments = appointments.Where(a => a.UserAccountId == userId).ToList();

                // Could also get additional service details if needed
                // var services = await _serviceProviders.ServicesNhanVtService.GetAllAsync();

                return userAppointments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get user appointments: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Example: Create appointment with status validation
        /// </summary>
        public async Task<bool> CreateAppointmentWithValidationAsync(AppointmentsTienDm appointment)
        {
            try
            {
                // Validate appointment status exists
                var appointmentStatus = await _serviceProviders.AppointmentStatusesTienDmService.GetByIdAsync(appointment.AppointmentStatusesTienDmid);
                if (appointmentStatus == null)
                {
                    throw new Exception("Invalid appointment status");
                }

                // Validate service exists
                var service = await _serviceProviders.ServicesNhanVtService.GetByIdAsync(appointment.ServicesNhanVtid);
                if (service == null)
                {
                    throw new Exception("Invalid service");
                }

                // Create appointment
                var result = await _serviceProviders.AppointmentsTienDmService.CreateAsync(appointment);

                return result > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create appointment: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Example: Get dashboard data using multiple services
        /// </summary>
        public async Task<object> GetDashboardDataAsync()
        {
            try
            {
                // Use multiple services to gather dashboard information
                var appointments = await _serviceProviders.AppointmentsTienDmService.GetAllAsync();
                var services = await _serviceProviders.ServicesNhanVtService.GetAllAsync();
                var statuses = await _serviceProviders.AppointmentStatusesTienDmService.GetAllAsync();

                return new
                {
                    TotalAppointments = appointments.Count,
                    TotalServices = services.Count,
                    TotalStatuses = statuses.Count,
                    RecentAppointments = appointments.Take(5).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get dashboard data: {ex.Message}", ex);
            }
        }
    }
}
