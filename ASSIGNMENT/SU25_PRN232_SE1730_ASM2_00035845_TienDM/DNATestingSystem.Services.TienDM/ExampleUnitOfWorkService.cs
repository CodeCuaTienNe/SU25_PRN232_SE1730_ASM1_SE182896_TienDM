using DNATestingSystem.Repository.TienDM;
using DNATestingSystem.Repository.TienDM.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DNATestingSystem.Services.TienDM
{
    /// <summary>
    /// Example service class demonstrating how to use UnitOfWork pattern
    /// This shows how to perform multiple repository operations with built-in transaction management
    /// </summary>
    public class ExampleUnitOfWorkService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExampleUnitOfWorkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Example: Create an appointment with automatic transaction management
        /// UnitOfWork handles transaction internally
        /// </summary>
        public async Task<bool> CreateAppointmentWithTransactionAsync(AppointmentsTienDm appointment)
        {
            try
            {
                // Use repositories to prepare changes
                var appointmentId = await _unitOfWork.AppointmentsTienDmRepository.CreateAsync(appointment);

                // Update related entities if needed
                var userAccount = await _unitOfWork.UserAccountRepository.GetByIdAsync(appointment.UserAccountId);
                if (userAccount != null)
                {
                    userAccount.ModifiedDate = DateTime.Now;
                    await _unitOfWork.UserAccountRepository.UpdateAsync(userAccount);
                }

                // Save all changes with transaction (built-in transaction handling)
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                // UnitOfWork handles rollback automatically in SaveChangesWithTransactionAsync
                throw new Exception($"Failed to create appointment: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Example: Simple operation without explicit transaction
        /// </summary>
        public async Task<List<AppointmentsTienDm>> GetAllAppointmentsAsync()
        {
            return await _unitOfWork.AppointmentsTienDmRepository.GetAllAsync();
        }

        /// <summary>
        /// Example: Batch operation with automatic transaction handling
        /// </summary>
        public async Task<bool> BatchUpdateAppointmentStatusAsync(List<int> appointmentIds, int newStatusId)
        {
            try
            {
                foreach (var appointmentId in appointmentIds)
                {
                    var appointment = await _unitOfWork.AppointmentsTienDmRepository.GetByIdAsync(appointmentId);
                    if (appointment != null)
                    {
                        appointment.AppointmentStatusesTienDmid = newStatusId;
                        appointment.ModifiedDate = DateTime.Now;

                        // Repository handles the context changes
                        await _unitOfWork.AppointmentsTienDmRepository.UpdateAsync(appointment);
                    }
                }

                // Save all changes with built-in transaction management
                var result = await _unitOfWork.SaveChangesWithTransactionAsync();

                return result > 0;
            }
            catch (Exception ex)
            {
                // UnitOfWork handles rollback automatically
                throw new Exception($"Failed to batch update appointments: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Example: Synchronous operation with transaction
        /// </summary>
        public bool UpdateAppointmentStatus(int appointmentId, int newStatusId)
        {
            try
            {
                var appointment = _unitOfWork.AppointmentsTienDmRepository.GetById(appointmentId);
                if (appointment != null)
                {
                    appointment.AppointmentStatusesTienDmid = newStatusId;
                    appointment.ModifiedDate = DateTime.Now;

                    _unitOfWork.AppointmentsTienDmRepository.Update(appointment);

                    // Save with built-in transaction handling
                    var result = _unitOfWork.SaveChangesWithTransaction();

                    return result > 0;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update appointment status: {ex.Message}", ex);
            }
        }
    }
}
