using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Services.TienDM;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DNATestingSystem.APIServices.BE.TienDM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsTienDMController : ControllerBase
    {
        private readonly IAppointmentsTienDmService _appointmentsTienDmService;

        public AppointmentsTienDMController(IAppointmentsTienDmService appointmentsTienDmService)
        {
            _appointmentsTienDmService = appointmentsTienDmService;
        }


        // GET: api/<AppointmentsTienDM>
        [HttpGet]
        public async Task<IEnumerable<AppointmentsTienDm>> Get()
        {
            return await _appointmentsTienDmService.GetAllAsyn();
        }

        // GET api/<AppointmentsTienDM>/5
        [HttpGet("{id}")]
        public async Task<AppointmentsTienDm> Get(int id)
        {
            return await _appointmentsTienDmService.GetByIdAsync(id);
        }

        // POST api/<AppointmentsTienDM>
        [HttpPost]
        public async Task<int> Create([FromBody] AppointmentsTienDm entity)
        {
            return await _appointmentsTienDmService.CreateAsync(entity);
        }

        // PUT api/<AppointmentsTienDM>/5
        [HttpPut("{id}")]
        public async Task<int> UpdateAsync(AppointmentsTienDm entity)
        {
            return await _appointmentsTienDmService.UpdateAsync(entity);
        }

        // DELETE api/<AppointmentsTienDM>/5
        [HttpDelete("{id}")]
        public async Task<bool> Search(int id)
        {
            return await _appointmentsTienDmService.DeleteAsync(id);
        }

        //SEARCH
        [HttpGet("{id}/{contactPhone}/{totalAmount}")]
        public async Task<List<AppointmentsTienDm>> Search(int id, string contactPhone, decimal totalAmount)
        {
            return await _appointmentsTienDmService.SearchAsync(id, contactPhone, totalAmount);
        }
    }
}
