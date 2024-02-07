using Trade.Domain.Entities;

namespace Trade.Domain.Interfaces
{
    public interface ITradeRepository
    {
        Task InsertVentasApp(List<VentasAppFromTrade> transactionTrade);
        Task UpdateVentasApp(List<VentasAppFromTrade> transactionsTrade);
        Task DeleteVentasApp(List<VentasAppFromTrade> transactionsTrade);
    }
}
