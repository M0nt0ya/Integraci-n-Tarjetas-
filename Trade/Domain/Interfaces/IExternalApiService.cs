using Trade.Domain.Entities;

namespace Trade.Domain.Interfaces
{
    public interface IExternalApiService
    {
        Task<List<TransactionTrade>> GetTransactions(string? fechaInicio = null, string? fechaFin = null);
        
    }
}
