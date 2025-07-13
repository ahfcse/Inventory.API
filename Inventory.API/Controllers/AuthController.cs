using Inventory.API.Interfaces;
using Inventory.API.Models;
using Inventory.API.Repositories;
using Inventory.API.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthService authService, IUserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel loginModel) // Renamed from Login
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _authService.AuthenticateAsync(loginModel);
            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationModel model) // Renamed from Register
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = PasswordHasher.HashPassword(model.Password),
                Role = "User"
            };

            try
            {
                var createdUser = await _userRepository.AddUserAsync(user);
                return CreatedAtAction(nameof(Authenticate), new { username = createdUser.Username }, null);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("me")]
        //[Authorize]
        public IActionResult GetCurrentUserInfo() // Renamed from GetCurrentUser
        {
            var username = User.Identity.Name;
            return Ok(new { Username = username });
        }
    }
}
