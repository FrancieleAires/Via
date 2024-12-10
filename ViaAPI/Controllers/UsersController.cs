using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaAPI.Data;
using ViaAPI.Models;
using ViaAPI.Services.UserService;
using ViaAPI.ViewModel;

namespace ViaAPI.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly ApiDbContext _dbContext;
        private readonly IUserService _userService;

        public UsersController(ApiDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _userService = userService;
        }

        [HttpGet("current")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userService.GetCurrentUserAsync();

            if (user == null)
            {
                return NotFound(new { message = "Usuário não encontrado." });
            }

            return Ok(user);
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserViewModel updateUserViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateUserDataAsync(updateUserViewModel);

            if (result.IsSuccess)
                return Ok(new { message = result.Value, token = result.Token });

            return BadRequest(result.Errors);
        }

    }
}
