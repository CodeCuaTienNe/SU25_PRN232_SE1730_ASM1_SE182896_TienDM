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
    //[Authorize(Roles = "1,2")]
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
        }        // GET api/AppointmentsTienDM/paginated - Get all appointments with pagination
        [HttpGet("paginated")]
        public async Task<ActionResult<PaginationResult<List<AppointmentsTienDm>>>> GetAllPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            // Create empty search request to get all items
            var searchRequest = new SearchAppointmentsTienDm
            {
                CurrentPage = page,
                PageSize = pageSize
            };

            var result = await _appointmentsTienDmService.SearchAsync(searchRequest);
            return Ok(result);
        }

        // POST api/AppointmentsTienDM/paginated - Get all appointments with pagination (POST version)
        [HttpPost("paginated")]
        public async Task<ActionResult<PaginationResult<List<AppointmentsTienDm>>>> GetAllPaginatedPost([FromBody] PaginationRequest paginationRequest)
        {
            if (paginationRequest == null)
            {
                return BadRequest("Pagination request cannot be null");
            }

            // Create empty search request to get all items
            var searchRequest = new SearchAppointmentsTienDm
            {
                CurrentPage = paginationRequest.Page ?? 1,
                PageSize = paginationRequest.PageSize ?? 10
            };

            var result = await _appointmentsTienDmService.SearchAsync(searchRequest);
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

            // Auto-assign UserAccountId from JWT token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                entity.UserAccountId = userId;
            }
            else
            {
                return BadRequest("User ID not found in token or invalid format");
            }

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
        }        // DELETE api/AppointmentsTienDM/{id} - Delete appointment
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _appointmentsTienDmService.DeleteAsync(id);
            if (result)
                return Ok(true);
            return NotFound();
        }

        // POST api/AppointmentsTienDM/search - Search appointments using SearchRequest model
        [HttpPost("search")]
        public async Task<ActionResult<PaginationResult<List<AppointmentsTienDm>>>> Search([FromBody] SearchAppointmentsTienDm searchRequest)
        {
            if (searchRequest == null)
            {
                return BadRequest("Search request cannot be null");
            }

            // Set default values for pagination if not provided
            searchRequest.CurrentPage ??= 1;
            searchRequest.PageSize ??= 10;

            var result = await _appointmentsTienDmService.SearchAsync(searchRequest);
            return Ok(result);
        }        // POST api/AppointmentsTienDM/search/all - Search appointments and return all results (non-paginated)
        [HttpPost("search/all")]
        public async Task<ActionResult<List<AppointmentsTienDm>>> SearchAll([FromBody] SearchAppointmentsTienDm searchRequest)
        {
            if (searchRequest == null)
            {
                return BadRequest("Search request cannot be null");
            }

            // Set large page size to get all results
            searchRequest.CurrentPage = 1;
            searchRequest.PageSize = 10000;

            var result = await _appointmentsTienDmService.SearchAsync(searchRequest);
            return Ok(result?.Items ?? new List<AppointmentsTienDm>());
        }

        // POST api/AppointmentsTienDM/search/simple - Simple search with basic parameters (creates SearchRequest internally)
        [HttpPost("search/simple")]
        public async Task<ActionResult<PaginationResult<List<AppointmentsTienDm>>>> SearchSimple([FromBody] SimpleSearchRequest request)
        {
            if (request == null)
            {
                return BadRequest("Search request cannot be null");
            }

            var searchRequest = new SearchAppointmentsTienDm
            {
                AppointmentsTienDmid = request.Id,
                ContactPhone = request.ContactPhone,
                TotalAmount = request.TotalAmount,
                CurrentPage = request.Page ?? 1,
                PageSize = request.PageSize ?? 10
            };

            var result = await _appointmentsTienDmService.SearchAsync(searchRequest);
            return Ok(result);
        }
    }    /// <summary>
         /// Simple search request model for basic search operations
         /// </summary>
    public class SimpleSearchRequest
    {
        public int? Id { get; set; }
        public string? ContactPhone { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }

    /// <summary>
    /// Pagination request model for basic pagination operations
    /// </summary>
    public class PaginationRequest
    {
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }
}
