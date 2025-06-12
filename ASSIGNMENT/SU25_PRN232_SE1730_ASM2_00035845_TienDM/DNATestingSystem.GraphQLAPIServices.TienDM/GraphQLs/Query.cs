using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Services.TienDM;

namespace DNATestingSystem.GraphQLAPIServices.TienDM.GraphQLs
{
    public class Query
    {
        private readonly IServiceProviders _serviceProviders;

        public Query(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        public async Task<List<AppointmentsTienDm>> GetAllAppointments()
        {
            try
            {
                var result = await _serviceProviders.AppointmentsTienDmService.GetAllAsync();
                return result;
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return new List<AppointmentsTienDm>();
            }
        }
    }
}
