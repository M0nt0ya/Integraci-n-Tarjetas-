using Microsoft.AspNetCore.Mvc;

using Trade.Infrastructure.Repository.Querys;

namespace Trade.Infrastructure.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasAppController(VentasAppRepository repository, ILogger<VentasAppController> logger) : ControllerBase
    {
        private readonly VentasAppRepository _repository = repository;
        private readonly ILogger<VentasAppController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _repository.QueryData();

                if (result != null)
                {
                    _logger.LogInformation("GET request received");
                    // Aquí puedes manejar los resultados de tu consulta y devolver una respuesta
                    return Ok(result);
                }
                else
                {
                    // Manejar el caso en que no se encontraron datos
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GET request");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("SearchTransactionsByDate/{date}")]
        public async Task<IActionResult> SearchTransactionsByDate(string date)
        {
            try
            {
                // Crea una instancia de MixTypeOrTuple con la fecha
                var dateStringToDateTime = DateTime.Parse(date);

                var result = await _repository.SearchTransactionsByDate(dateStringToDateTime);

                if (result != null)
                {
                    _logger.LogInformation("GET request received for SearchTransactionsByDate");
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing GET request for SearchTransactionsByDate");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}