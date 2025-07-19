//using DNATestingSystem.Repository.TienDM.Models;
//using DNATestingSystem.Services.TienDM;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace DNATestingSystem.APIServices.BE.TienDM.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    [Authorize(Roles = "1,2")]
//    public class SystemUserAccountsController : ControllerBase
//    {
//        private readonly ISystemUserAccountService _systemUserAccountService;

//        public SystemUserAccountsController(ISystemUserAccountService systemUserAccountService)
//        {
//            _systemUserAccountService = systemUserAccountService;
//        }

//        // GET api/SystemUserAccounts - Get all user accounts
//        [HttpGet]
//        public async Task<ActionResult<List<SystemUserAccount>>> GetAllUserAccounts()
//        {
//            var users = await _systemUserAccountService.GetAllAsync();
//            return Ok(users);
//        }

//        // GET api/SystemUserAccounts/{id} - Get user account by ID
//        [HttpGet("{id}")]
//        public async Task<ActionResult<SystemUserAccount>> GetById(int id)
//        {
//            var user = await _systemUserAccountService.GetUserAccountById(id);
//            if (user?.UserAccountId == 0)
//                return NotFound();
//            return Ok(user);
//        }

//        // POST api/SystemUserAccounts - Create new user account
//        [HttpPost]
//        public async Task<ActionResult<int>> Create([FromBody] SystemUserAccount entity)
//        {
//            var result = await _systemUserAccountService.CreateAsync(entity);
//            return Ok(result);
//        }

//        // PUT api/SystemUserAccounts/{id} - Update existing user account
//        [HttpPut("{id}")]
//        public async Task<ActionResult<int>> Update(int id, [FromBody] SystemUserAccount entity)
//        {
//            entity.UserAccountId = id;
//            var result = await _systemUserAccountService.UpdateAsync(entity);
//            return Ok(result);
//        }

//        // DELETE api/SystemUserAccounts/{id} - Delete user account
//        [HttpDelete("{id}")]
//        public async Task<ActionResult<bool>> Delete(int id)
//        {
//            var result = await _systemUserAccountService.DeleteAsync(id);
//            return Ok(result);
//        }
//    }
//}
