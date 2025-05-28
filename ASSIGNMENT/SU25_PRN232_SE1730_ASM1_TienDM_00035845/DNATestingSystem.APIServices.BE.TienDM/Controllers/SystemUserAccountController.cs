using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Services.TienDM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DNATestingSystem.APIServices.BE.TienDM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUserAccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ISystemUserAccountService _userAccountsService;

        public SystemUserAccountController(IConfiguration config, ISystemUserAccountService userAccountsService)
        {
            _config = config;
            _userAccountsService = userAccountsService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userAccountsService.GetUserAccount(request.UserName, request.Password);

            if (user == null)
            //    return Unauthorized(new { message = "Invalid username or password" });

            //// Check if user role is 1 or 2 (only these roles can access login)
            //if (user.RoleId != 1 && user.RoleId != 2)
            //    return StatusCode(403, new { message = "Access denied. Only administrators and staff can login." });

            //// Check if user account is active
            //if (!user.IsActive)
                return Unauthorized(new { message = "Account is deactivated" });

            var token = GenerateJSONWebToken(user);

            //return Ok(new { 
            //    token = token,
            //    user = new {
            //        userId = user.UserAccountId,
            //        userName = user.UserName,
            //        fullName = user.FullName,
            //        email = user.Email,
            //        role = user.RoleId
            //    },
            //    expiresIn = 120 // minutes
            //});
            return Ok(token);
        }

        private string GenerateJSONWebToken(SystemUserAccount systemUserAccount)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                new Claim[]
                {
                    new(ClaimTypes.Name, systemUserAccount.UserName),
                    new(ClaimTypes.Role, systemUserAccount.RoleId.ToString())
                },
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public sealed record LoginRequest(string UserName, string Password);
    }
}
