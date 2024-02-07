using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Trade.Application;

namespace Trade.Infrastructure.BackgroundServices
{
    public class CronJobService(IServiceProvider serviceProvider, IConfiguration cronJobSettings) : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        private readonly string IntervalTrade = cronJobSettings["CronJobSettings:IntervalTrade"] ?? "5";


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var tradeAppService = scope.ServiceProvider.GetRequiredService<TradeAppService>();
                        await tradeAppService.ProcessTrades();
                    }
                    // Espera el intervalo especificado en la configuración antes de la próxima ejecución
                    await Task.Delay(TimeSpan.FromMinutes(int.Parse(IntervalTrade)), stoppingToken);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error executing: " + ex.Message);
                //throw;
            }
            finally
            {
                Console.WriteLine("Cron ejecutado.");
            }

        }
    }

}
