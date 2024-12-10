using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViaAPI.Models;
using ViaAPI.Services.FeedbackService;

namespace ViaAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedbackController(IFeedbackService feedbackService, IHttpContextAccessor contextAccessor)
        {
            _feedbackService = feedbackService;
            _httpContextAccessor = contextAccessor;
        }
        [Authorize]
        [HttpPost("create-feedback")]
        public async Task<IActionResult> CreateFeedbackAsync(FeedbackViewModel feedbackViewModel)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Usuário não autenticado.");
            }
            var userId = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))!.Value;

            if (userId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var userGuid = Guid.Parse(userId);
            var result = await _feedbackService.CreateFeedbackAsync(feedbackViewModel, userGuid);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

       
        [Authorize]
        [HttpGet("history")]
        public async Task<IActionResult> HistoryFeedbackAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"))!.Value;
            if (userId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            var userGuid = Guid.Parse(userId);
            var result = await _feedbackService.HistoryFeedback(userGuid);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Não encontrado");
            }
        }

    }
}
