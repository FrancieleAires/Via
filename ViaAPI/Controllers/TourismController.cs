using Microsoft.AspNetCore.Mvc;
using ViaAPI.Data;
using ViaAPI.Models;
using ViaAPI.Services.TourismService;

namespace ViaAPI.Controllers
{
    [ApiController]
    [Route("api/pontoTuristico")]
    public class TourismController : ControllerBase
    {
        private readonly ApiDbContext _apiDbContext;
        private readonly ITourismService _tourismService;


        public TourismController(ApiDbContext apiDbContext, ITourismService tourismService)
        {
            _apiDbContext = apiDbContext;
            _tourismService = tourismService;

        }

        [HttpGet]
        public IActionResult GetAllTourism()
        {
            var pontosTuristicos = _tourismService.GetAllTourism();

            if (pontosTuristicos == null || !pontosTuristicos.Any())
            {
                return NotFound(new { message = "Nenhum ponto turístico encontrado." });
            }

            return Ok(pontosTuristicos);
        }
        [HttpGet("search")]
        public IActionResult SearchTourism([FromQuery] string query)
        {
            var pontosTuristicos = _tourismService.SearchTourism(query);

            if (pontosTuristicos == null || !pontosTuristicos.Any())
            {
                return NotFound(new { message = "Nenhum ponto turístico correspondente foi encontrado." });
            }

            return Ok(pontosTuristicos);
        }
    }
}
