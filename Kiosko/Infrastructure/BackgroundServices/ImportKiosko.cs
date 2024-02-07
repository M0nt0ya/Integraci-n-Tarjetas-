using Kiosko.Aplication.Services;
using Kiosko.Domain.Entities;

namespace Kiosko.Infrastructure.BackgroundServices
{
    public class ImportarVentasKiosko
    {
        private readonly ILogger<ImportarVentasKiosko> _logger;
        private readonly KioskoWsService _kioskoWsService;
        private readonly KioskoApp _kiosko;
        private readonly DateTime? _fechaInicio;
        private readonly DateTime? _fechaFin;

        public ImportarVentasKiosko(
            ILogger<ImportarVentasKiosko> logger,
            KioskoWsService kioskoWsService,
            KioskoApp kiosko,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _kioskoWsService = kioskoWsService ?? throw new ArgumentNullException(nameof(kioskoWsService));
            _kiosko = kiosko ?? throw new ArgumentNullException(nameof(kiosko));
            _fechaInicio = fechaInicio;
            _fechaFin = fechaFin;
        }

        public void Execute()
        {
            try
            {
                // Utiliza el logger específico para ImportarVentasKiosko
                var ventasKioskoService = new KioskoWsService(_logger, _kiosko);
                ventasKioskoService.ImportarVentas(
                    _fechaInicio.GetValueOrDefault(),
                    _fechaFin.GetValueOrDefault()
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    $"Error en el servicio ImportarVentasKiosko: {ex.Message}"
                );
            }
        }
    }
}
