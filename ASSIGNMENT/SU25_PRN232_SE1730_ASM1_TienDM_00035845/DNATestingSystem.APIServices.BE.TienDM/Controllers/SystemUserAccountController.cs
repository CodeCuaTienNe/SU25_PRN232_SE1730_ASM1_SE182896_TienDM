using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DNATestingSystem.Repository.TienDM.Models;
using DNATestingSystem.Services.TienDM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DNATestingSystem.APIServices.BE.TienDM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]    public class SystemUserAccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ISystemUserAccountService _userAccountsService;

        public SystemUserAccountController(IConfiguration config, ISystemUserAccountService userAccountsService)
        {
            _config = config;
            _userAccountsService = userAccountsService;
        }
        
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userAccountsService.GetUserAccount(request.UserName, request.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid username or password" });

            // Check if user role is 1 or 2 (only these roles can access login)
            if (user.RoleId != 1 && user.RoleId != 2)
                return StatusCode(403, new { message = "Access denied. Only administrators and staff can login." });

            // Check if user account is active
            if (!user.IsActive)
                return Unauthorized(new { message = "Account is deactivated" });

            var token = GenerateJSONWebToken(user);

            return Ok(new { 
                token = token,
                user = new {
                    userId = user.UserAccountId,
                    userName = user.UserName,
                    fullName = user.FullName,
                    email = user.Email,
                    role = user.RoleId
                },
                expiresIn = 120 // minutes
            });
        }

        private string GenerateJSONWebToken(SystemUserAccount systemUserAccount)
        {
            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT Key is not configured");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, systemUserAccount.UserAccountId.ToString()),
                new(ClaimTypes.Name, systemUserAccount.UserName ?? ""),
                new(ClaimTypes.Role, systemUserAccount.RoleId.ToString()),
                new("UserId", systemUserAccount.UserAccountId.ToString()),
                new("RoleId", systemUserAccount.RoleId.ToString())
            };

            // Add email claim if available
            if (!string.IsNullOrEmpty(systemUserAccount.Email))
                claims.Add(new Claim(ClaimTypes.Email, systemUserAccount.Email));

            // Add full name claim if available
            if (!string.IsNullOrEmpty(systemUserAccount.FullName))
                claims.Add(new Claim("FullName", systemUserAccount.FullName));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }        [HttpGet("Profile")]
        [Authorize(Roles = "1,2")] // Only allow roles 1 and 2
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { message = "Invalid token" });

            var user = await _userAccountsService.GetUserAccountById(userId);
            
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new {
                userId = user.UserAccountId,
                userName = user.UserName,
                fullName = user.FullName,
                email = user.Email,
                phone = user.Phone,
                employeeCode = user.EmployeeCode,
                role = user.RoleId,
                isActive = user.IsActive,
                createdDate = user.CreatedDate
            });
        }

        public sealed record LoginRequest(string UserName, string Password);
    }
}
