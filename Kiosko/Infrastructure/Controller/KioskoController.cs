using Kiosko.Application.Services;
using Kiosko.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Kiosko.Infrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KioskoController : ControllerBase
    {
        private readonly KioskoServicesMethods _kioskoServicesMethods;

        public KioskoController(
            KioskoServicesMethods kioskoServicesMethods
        )
        {
            _kioskoServicesMethods = kioskoServicesMethods;
        }

        [HttpPost]
        public IActionResult CreateKiosko([FromBody] KioskoApp kioskoRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kioskoApp = new KioskoApp
            {
                IdLocal = kioskoRequest.IdLocal,
                Direccion = kioskoRequest.Direccion,
                Email = kioskoRequest.Email,
                Clave = kioskoRequest.Clave,
                Puerto = kioskoRequest.Puerto,
                TokenAcceso = kioskoRequest.TokenAcceso,
                TokenFechaCaduca = kioskoRequest.TokenFechaCaduca,
                TokenFechaCreado = kioskoRequest.TokenFechaCreado,
                Estado = kioskoRequest.Estado
            };

            _kioskoServicesMethods.CreateKiosko(kioskoApp);

            var mensaje = "El Kiosko se ha creado exitosamente.";

            return Ok(new { Mensaje = mensaje });
        }

        [HttpPut("{kioskoId}")]
        public IActionResult UpdateKiosko(int kioskoId, [FromBody] KioskoApp kioskoRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingKiosko = _kioskoServicesMethods.GetByIdKiosko(kioskoId);

            if (existingKiosko == null)
            {
                return NotFound();
            }

            existingKiosko.Direccion = kioskoRequest.Direccion;
            existingKiosko.Email = kioskoRequest.Email;
            existingKiosko.Clave = kioskoRequest.Clave;
            existingKiosko.Puerto = kioskoRequest.Puerto;
            existingKiosko.TokenAcceso = kioskoRequest.TokenAcceso;
            existingKiosko.TokenFechaCaduca = kioskoRequest.TokenFechaCaduca;
            existingKiosko.TokenFechaCreado = kioskoRequest.TokenFechaCreado;
            existingKiosko.Estado = kioskoRequest.Estado;

            _kioskoServicesMethods.UpdateKiosko(existingKiosko);

            var mensaje = $"El Kiosko con ID {kioskoId} se ha actualizado exitosamente.";

            return Ok(new { Mensaje = mensaje });
        }

        [HttpGet("{kioskoId}")]
        public IActionResult GetByIdKiosko(int kioskoId)
        {
            var kiosko = _kioskoServicesMethods.GetByIdKiosko(kioskoId);

            if (kiosko == null)
            {
                return NotFound();
            }

            return Ok(kiosko);
        }
    }
}
