using System;
using Kiosko.Infrastructure.BackgroundServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KioskoJobsController : ControllerBase
    {
        private readonly ILogger<KioskoJobsController> _logger;
        private readonly ILogger<ImportarVentasKiosko> _jobLogger;

        public KioskoJobsController(ILogger<KioskoJobsController> logger, ILogger<ImportarVentasKiosko> jobLogger)
        {
            _logger = logger;
            _jobLogger = jobLogger;
        }

        [HttpPost("kioskoReprocess")]
        public IActionResult KioskoReprocess([FromBody] KioskoReprocessRequest request)
        {
            try
            {
                DateTime fromDate = request.From;
                DateTime toDate = request.To;

                // Utiliza el nuevo logger específico para ImportarVentasKiosko
                var job = new ImportarVentasKiosko(_jobLogger, null, null, fromDate, toDate);

                // Realiza la lógica del trabajo (ajusta según tu implementación)
                job.Execute();

                return Ok(job);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante kioskoReprocess");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }

    public class KioskoReprocessRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
