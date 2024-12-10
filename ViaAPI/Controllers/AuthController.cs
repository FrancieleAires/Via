using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ViaAPI.Models;
using ViaAPI.Services.UserService;
using ViaAPI.ViewModel;



namespace viaAPI.Controllers
{
    [ApiController]
    [Route("api/conta")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IUserService _userService;


        public AuthController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              IOptions<JwtSettings> jwtSettings,
                              IUserService userService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _userService = userService;

        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);
            var resultado = await _userService.RegisterAsync(registerUser);

            if (resultado.IsSuccess)
            {
                return Ok(resultado.Value);
            }

            return BadRequest(new { Error = "Falha ao registrar o usuário", Details = resultado.Errors });
        }


        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var resultado = await _userService.LoginAsync(loginUser);

            if (resultado.IsSuccess)
            {
                return Ok(new { Token = resultado.Value });
            }

            return Unauthorized(new { Error = resultado.Errors.FirstOrDefault() });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel forgotPasswordViewModel)
        {
            var result = await _userService.ForgotPasswordAsync(forgotPasswordViewModel.Email);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel resetPasswordViewModel)
        {
            var result = await _userService.ResetPasswordAsync(resetPasswordViewModel.Email, resetPasswordViewModel.Token, resetPasswordViewModel.NewPassword);
            if (!result.IsSuccess)
                return BadRequest(result.Errors);

            return Ok(result.Value);
        }
    }
}


