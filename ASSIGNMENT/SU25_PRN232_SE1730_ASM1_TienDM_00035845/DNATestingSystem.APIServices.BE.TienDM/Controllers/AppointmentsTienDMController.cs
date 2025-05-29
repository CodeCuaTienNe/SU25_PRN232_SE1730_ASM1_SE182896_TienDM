using DNATestingSystem.Repository.TienDM.ModelExtensions;
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
    public class AppointmentsTienDMController : ControllerBase
    {
        private readonly IAppointmentsTienDmService _appointmentsTienDmService;

        public AppointmentsTienDMController(IAppointmentsTienDmService appointmentsTienDmService)
        {
            _appointmentsTienDmService = appointmentsTienDmService;
        }        
        
        // GET api/AppointmentsTienDM - Get all appointments
        [HttpGet]
        public async Task<ActionResult<List<AppointmentsTienDm>>> GetAll()
        {
            var appointments = await _appointmentsTienDmService.GetAllAsync();
            return Ok(appointments);
        }        
        
        // GET api/AppointmentsTienDM/paginated - Get all appointments with pagination
        [HttpGet("paginated")]
        public async Task<ActionResult<PaginationResult<List<AppointmentsTienDm>>>> GetAllPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            // Use proper GetAllPaginated method
            var result = await _appointmentsTienDmService.GetAllPaginatedAsync(page, pageSize);
            return Ok(result);
        }

        // GET api/AppointmentsTienDM/{id} - Get appointment by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentsTienDm>> GetById(int id)
        {
            var appointment = await _appointmentsTienDmService.GetByIdAsync(id);
            if (appointment?.AppointmentsTienDmid == 0)
                return NotFound();
            return Ok(appointment);
        }



        // POST api/AppointmentsTienDM - Create new appointment
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] AppointmentsTienDm entity)
        {
            // Ensure ID is not set (auto-generated)
            entity.AppointmentsTienDmid = 0;

            var result = await _appointmentsTienDmService.CreateAsync(entity);
            if (result > 0)
                return CreatedAtAction(nameof(GetById), new { id = result }, result);
            return BadRequest();
        }

        // PUT api/AppointmentsTienDM/{id} - Update existing appointment
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update(int id, [FromBody] AppointmentsTienDm entity)
        {
            // Set the ID from route parameter
            entity.AppointmentsTienDmid = id;

            var result = await _appointmentsTienDmService.UpdateAsync(entity);
            if (result > 0)
                return Ok(result);
            return NotFound();
        }     
           // DELETE api/AppointmentsTienDM/{id} - Delete appointment
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _appointmentsTienDmService.DeleteAsync(id);
            if (result)
                return Ok(true);
            return NotFound();
        }       
          // GET api/AppointmentsTienDM/search - Search appointments without pagination (returns simple list)
        [HttpGet("search")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<List<AppointmentsTienDm>>> Search(
            [FromQuery] int id = 0,
            [FromQuery] string contactPhone = "",
            [FromQuery] decimal totalAmount = 0)
        {
            // Get first 1000 items from search for simple list (non-paginated)
            var result = await _appointmentsTienDmService.SearchAsync(id, contactPhone ?? "", totalAmount, 1, 1000);
            return Ok(result.Items);
        }

        // GET api/AppointmentsTienDM/search/paginated - Search appointments with pagination
        [HttpGet("search/paginated")]
        [Authorize(Roles = "1,2")]
        public async Task<ActionResult<PaginationResult<List<AppointmentsTienDm>>>> SearchPaginated(
            [FromQuery] int id = 0,
            [FromQuery] string contactPhone = "",
            [FromQuery] decimal totalAmount = 0,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            // Search with pagination
            var result = await _appointmentsTienDmService.SearchAsync(id, contactPhone ?? "", totalAmount, page, pageSize);
            return Ok(result);
        }


        // // GET api/AppointmentsTienDM/search?id=1&contactPhone=123&totalAmount=100 - Search appointments
        // [HttpGet("search")]
        // public async Task<ActionResult<List<AppointmentsTienDm>>> Search(
        //    [FromQuery] int id = 0,
        //    [FromQuery] string contactPhone = "",
        //    [FromQuery] decimal totalAmount = 0)
        // {
        //    var appointments = await _appointmentsTienDmService.SearchAsync(id, contactPhone ?? "", totalAmount);
        //    return Ok(appointments);
        // }

    }
}
