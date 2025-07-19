//using DNATestingSystem.Repository.TienDM.Models;
//using DNATestingSystem.Services.TienDM;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace DNATestingSystem.APIServices.BE.TienDM.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize(Roles = "1,2")]
//    public class ServicesNhanVtController : ControllerBase
//    {
//        private readonly IServicesNhanVtService _servicesNhanVtService;

//        public ServicesNhanVtController(IServicesNhanVtService servicesNhanVtService)
//        {
//            _servicesNhanVtService = servicesNhanVtService;
//        }

//        // GET api/ServicesNhanVt - Get all services
//        [HttpGet]
//        public async Task<ActionResult<List<ServicesNhanVt>>> GetAllServicesNhanVt()
//        {
//            var services = await _servicesNhanVtService.GetAllAsync();
//            return Ok(services);
//        }

//        // GET api/ServicesNhanVt/{id} - Get service by ID
//        [HttpGet("{id}")]
//        public async Task<ActionResult<ServicesNhanVt>> GetById(int id)
//        {
//            var service = await _servicesNhanVtService.GetByIdAsync(id);
//            if (service?.ServicesNhanVtid == 0)
//                return NotFound();
//            return Ok(service);
//        }

//        // GET api/ServicesNhanVt/active - Get all active services
//        [HttpGet("active")]
//        [Authorize(Roles = "1,2")]
//        public async Task<ActionResult<List<ServicesNhanVt>>> GetActiveServices()
//        {
//            var services = await _servicesNhanVtService.GetActiveServicesAsync();
//            return Ok(services);
//        }

//        // GET api/ServicesNhanVt/search - Search services
//        [HttpGet("search")]
//        [Authorize(Roles = "1,2")]
//        public async Task<ActionResult<List<ServicesNhanVt>>> Search(
//            [FromQuery] int id = 0,
//            [FromQuery] string serviceName = "")
//        {
//            var services = await _servicesNhanVtService.SearchAsync(id, serviceName ?? "");
//            return Ok(services);
//        }

//        // POST api/ServicesNhanVt - Create new service
//        [HttpPost]
//        public async Task<ActionResult<int>> Create([FromBody] ServicesNhanVt entity)
//        {
//            var result = await _servicesNhanVtService.CreateAsync(entity);
//            return Ok(result);
//        }

//        // PUT api/ServicesNhanVt/{id} - Update existing service
//        [HttpPut("{id}")]
//        public async Task<ActionResult<int>> Update(int id, [FromBody] ServicesNhanVt entity)
//        {
//            entity.ServicesNhanVtid = id;
//            var result = await _servicesNhanVtService.UpdateAsync(entity);
//            return Ok(result);
//        }

//        // DELETE api/ServicesNhanVt/{id} - Delete service
//        [HttpDelete("{id}")]
//        public async Task<ActionResult<bool>> Delete(int id)
//        {
//            var result = await _servicesNhanVtService.DeleteAsync(id);
//            return Ok(result);
//        }
//    }
//}
