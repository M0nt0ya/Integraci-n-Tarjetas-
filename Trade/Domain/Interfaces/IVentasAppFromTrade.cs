using Shared.Domain.Entities;
using Trade.Domain.Entities;
namespace Trade.Domain.Interfaces
{
    public interface IVentasAppFromTrade
    {
        VentasApp ConvertTransactionTradeToVentasApp(ITransactionTrade transactionTrade);
        string? ValidateBrand(string brand);
        string? ValidateAcquirerID(string? acquirerId);
        string? FormatAddress(string? address);
        string? ParseAuthorizationCode(string? purchaseOperationNumber, string? authorizationCode, string? IdAcquirer);
    }
}