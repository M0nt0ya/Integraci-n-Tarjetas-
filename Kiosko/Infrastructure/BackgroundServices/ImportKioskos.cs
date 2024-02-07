using Kiosko.Aplication.Services;
using Kiosko.Application.Services;

namespace Kiosko.Infrastructure.BackgroundServices
{
    public class ImportarVentasKioskos : BackgroundService
    {
        private readonly ILogger<ImportarVentasKiosko> _logger;
        private readonly KioskoWsService _kioskoWsService;
        private readonly KioskoServicesMethods _kioskoServicesMethods;

        private DateTime fechaInicio;
        private DateTime fechaFin;

        public ImportarVentasKioskos(
            ILogger<ImportarVentasKiosko> logger,
            KioskoWsService kioskoWsService,
            KioskoServicesMethods kioskoServicesMethods,
            DateTime? fechaInicio = null,
            DateTime? fechaFin = null
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _kioskoWsService =
                kioskoWsService ?? throw new ArgumentNullException(nameof(kioskoWsService));
            _kioskoServicesMethods =
                kioskoServicesMethods
                ?? throw new ArgumentNullException(nameof(kioskoServicesMethods));
            this.fechaInicio = fechaInicio ?? DateTime.Now.AddDays(-1);
            this.fechaFin = fechaFin ?? DateTime.Now.AddDays(-1);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var kioskos = _kioskoServicesMethods.GetAllKioskos();

                Parallel.ForEach(
                    kioskos,
                    (kiosko) =>
                    {
                        var job = new ImportarVentasKiosko(
                            _logger,
                            _kioskoWsService,
                            kiosko,
                            fechaInicio,
                            fechaFin
                        );
                        job.Execute();
                    }
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el servicio ImportarVentasKioskos: {ex.Message}");
            }
        }
    }
}
