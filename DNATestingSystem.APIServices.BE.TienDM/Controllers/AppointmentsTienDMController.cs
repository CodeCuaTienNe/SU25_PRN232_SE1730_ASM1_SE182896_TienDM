using DNATestingSystem.Repository.TienDM.ModelExtensions;
using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Services.TienDM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DNATestingSystem.APIServices.BE.TienDM.Controllers
{
    //implement OData
    [Route("api/[controller]")]
    [ApiController]

   
    public class AppointmentsTienDMController : ControllerBase
    {
        private readonly IAppointmentsTienDmService _appointmentsTienDmService;

        public AppointmentsTienDMController(IAppointmentsTienDmService appointmentsTienDmService)
        {
            _appointmentsTienDmService = appointmentsTienDmService;
        }

        [HttpGet("basic")]
        public async Task<ActionResult<List<AppointmentsTienDm>>> GetAll()
        {
            var appointments = await _appointmentsTienDmService.GetAllBasicAsync();
            return Ok(appointments);
        }

        [HttpGet("search")]
        [EnableQuery]
        public async Task<ActionResult<List<AppointmentsTienDmDto>>>Search()
        {
            var appointments =  _appointmentsTienDmService.GetAllAsync().Result.AsQueryable();
            return Ok(appointments);
        }


        [HttpGet("paginated")]
        [Authorize]
        public async Task<ActionResult<PaginationResult<List<AppointmentsTienDmDto>>>> GetAllPaginated([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var searchRequest = new SearchAppointmentsTienDm { CurrentPage = page, PageSize = pageSize };
            var result = await _appointmentsTienDmService.SearchAsync(searchRequest);
            return Ok(result);
        }

        // GET: api/AppointmentsTienDM/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppointmentsTienDmDto>> GetById(int id)
        {
            var appointment = await _appointmentsTienDmService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound();
            return Ok(appointment);
        }

        //Create
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] AppointmentsTienDmCreateRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            int? userId = null;
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
            {
                userId = parsedUserId;
            }
            var resultId = await _appointmentsTienDmService.CreateAsync(request, userId);
            return resultId > 0;
        }


        //Update
        [Authorize(Roles = "1,2")]
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Update(int id, [FromBody] AppointmentsTienDmCreateRequest request)
        {
            var result = await _appointmentsTienDmService.UpdateAsync(id, request);
            return result > 0;
        }

        [Authorize(Roles = "1,2")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var result = await _appointmentsTienDmService.DeleteAsync(id);
            return result;
        }

        [Authorize]
        [HttpPost("search")]
        public async Task<ActionResult<PaginationResult<List<AppointmentsTienDmDto>>>> Search([FromBody] SearchAppointmentsTienDm searchRequest)
        {
            if (searchRequest == null)
                return BadRequest("Search request cannot be null");
            searchRequest.CurrentPage ??= 1;
            searchRequest.PageSize ??= 10;
            var result = await _appointmentsTienDmService.SearchAsync(searchRequest);
            return Ok(result);
        }
    }
}
