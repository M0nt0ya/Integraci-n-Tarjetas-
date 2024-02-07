using System.Threading.Tasks;
using Trade.Domain.Entities;
using Trade.Domain.Interfaces;

namespace Trade.Application
{
    public class TradeAppService(ITradeRepository mutationsTrade, IExternalApiService externalApiService)
    {
        private readonly ITradeRepository _mutationsTrade = mutationsTrade;
        private readonly IExternalApiService _externalApiService = externalApiService;

        public async Task ProcessTrades()
        {
            try
            {
                var TransactionsTradeFromApi = await _externalApiService.GetTransactions();

                if (TransactionsTradeFromApi == null)
                {
                    Console.WriteLine("No se obtuvieron transacciones de la API");
                    return;
                }

                var ventasAppFromTrade = new VentasAppFromTrade();

                var ventasAppList = TransactionsTradeFromApi.Select(transaction =>
                {
                    Console.WriteLine(transaction.purchaseAmount);
                    var converted = ventasAppFromTrade.ConvertTransactionTradeToVentasApp(transaction);
                    return converted;
                }).ToList();

                await _mutationsTrade.InsertVentasApp(ventasAppList);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }



    }
}
