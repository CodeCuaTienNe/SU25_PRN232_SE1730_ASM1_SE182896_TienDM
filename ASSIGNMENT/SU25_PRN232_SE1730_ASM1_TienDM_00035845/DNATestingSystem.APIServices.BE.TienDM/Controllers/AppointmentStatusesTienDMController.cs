using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Services.TienDM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DNATestingSystem.APIServices.BE.TienDM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "1,2")]
    public class AppointmentStatusesTienDMController : ControllerBase
    {
        private readonly IAppointmentStatusesTienDmService _appointmentStatusesTienDmService;

        public AppointmentStatusesTienDMController(IAppointmentStatusesTienDmService appointmentStatusesTienDmService)
        {
            _appointmentStatusesTienDmService = appointmentStatusesTienDmService;
        }

        // GET api/AppointmentStatusesTienDM - Get all appointment statuses
        [HttpGet]
        public async Task<ActionResult<List<AppointmentStatusesTienDm>>> GetAllAppointmentStatusesTienDm()
        {
            var statuses = await _appointmentStatusesTienDmService.GetAllAsync();
            return Ok(statuses);
        }

        // GET api/AppointmentStatusesTienDM/{id} - Get appointment status by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentStatusesTienDm>> GetById(int id)
        {
            var status = await _appointmentStatusesTienDmService.GetByIdAsync(id);
            if (status?.AppointmentStatusesTienDmid == 0)
                return NotFound();
            return Ok(status);
        }

        // GET api/AppointmentStatusesTienDM/active - Get all active appointment statuses
        [HttpGet("active")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<List<AppointmentStatusesTienDm>>> GetActiveStatuses()
        {
            var statuses = await _appointmentStatusesTienDmService.GetActiveStatusesAsync();
            return Ok(statuses);
        }

        // GET api/AppointmentStatusesTienDM/search - Search appointment statuses
        [HttpGet("search")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<List<AppointmentStatusesTienDm>>> Search(
            [FromQuery] int id = 0,
            [FromQuery] string statusName = "")
        {
            var statuses = await _appointmentStatusesTienDmService.SearchAsync(id, statusName ?? "");
            return Ok(statuses);
        }

        // POST api/AppointmentStatusesTienDM - Create new appointment status
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] AppointmentStatusesTienDm entity)
        {
            // Ensure ID is not set (auto-generated)
            entity.AppointmentStatusesTienDmid = 0;

            var result = await _appointmentStatusesTienDmService.CreateAsync(entity);
            if (result > 0)
                return CreatedAtAction(nameof(GetById), new { id = result }, result);
            return BadRequest();
        }

        // PUT api/AppointmentStatusesTienDM/{id} - Update existing appointment status
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, [FromBody] AppointmentStatusesTienDm entity)
        {
            // Set the ID from route parameter
            entity.AppointmentStatusesTienDmid = id;

            var result = await _appointmentStatusesTienDmService.UpdateAsync(entity);
            if (result > 0)
                return Ok(result);
            return NotFound();
        }

        // DELETE api/AppointmentStatusesTienDM/{id} - Delete appointment status
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _appointmentStatusesTienDmService.DeleteAsync(id);
            if (result)
                return Ok(true);
            return NotFound();
        }
    }
}
