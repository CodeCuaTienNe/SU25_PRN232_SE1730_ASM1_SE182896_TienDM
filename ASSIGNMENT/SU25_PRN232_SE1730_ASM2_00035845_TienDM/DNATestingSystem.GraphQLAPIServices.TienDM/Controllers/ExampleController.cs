using Microsoft.AspNetCore.Mvc;
using DNATestingSystem.Services.TienDM;
using DNATestingSystem.Repository.TienDM.Models;

namespace DNATestingSystem.GraphQLAPIServices.TienDM.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        private readonly IServiceProviders _serviceProviders;

        public ExampleController(IServiceProviders serviceProviders)
        {
            _serviceProviders = serviceProviders;
        }

        /// <summary>
        /// Example REST endpoint using ServiceProviders
        /// GET api/example/users
        /// </summary>
        [HttpGet("users")]
        public async Task<ActionResult<List<SystemUserAccount>>> GetUsers()
        {
            try
            {
                var users = await _serviceProviders.SystemUserAccountService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Example REST endpoint for appointments
        /// GET api/example/appointments
        /// </summary>
        [HttpGet("appointments")]
        public async Task<ActionResult<List<AppointmentsTienDm>>> GetAppointments()
        {
            try
            {
                var appointments = await _serviceProviders.AppointmentsTienDmService.GetAllAsync();
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Example REST endpoint for services
        /// GET api/example/services
        /// </summary>
        [HttpGet("services")]
        public async Task<ActionResult<List<ServicesNhanVt>>> GetServices()
        {
            try
            {
                var services = await _serviceProviders.ServicesNhanVtService.GetAllAsync();
                return Ok(services);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
