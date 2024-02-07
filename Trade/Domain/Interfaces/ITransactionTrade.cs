namespace Trade.Domain.Interfaces
{
    public interface ITransactionTrade
    {
        int accountId { get; } //: 2,
        string accountName { get; }//: "Juan Valdez",
        int vendorExternalId { get; }//: 12,
        string vendorName { get; }//: "JUAN VALDEZ",
        string subtotal { get; }//: "18.5714",
        string discounts { get; }//: "0.0000",
        string taxes { get; }//: "2.2286",
        double iva { get; }//: 0.12,
        double commission { get; }//: 0.09,
        string bin { get; }//: "46857100",
        string brand { get; }//: "VISA",
        int errorCode { get; }//: 0,
        string? reserved1 { get; }//: null,
        string acquirerId { get; }//: "Kushki",
        string? idCommerce { get; }//: null,
        string reserved22 { get; }//: "debit",
        string? reserved23 { get; }//: null,
        string? shippingZIP { get; }//: null,
        string errorMessage { get; }//: "Approved",
        string IDTransaction { get; }//: "128233033289923451",
        string shippingEmail { get; }//: "stalincalderon2015@gmail.com",
        decimal purchaseAmount { get; }//: 20.8,
        string shippingAddress { get; }//: "  ",
        string? shippingCountry { get; }//: null,
        string shippingLastName { get; }//: "Calderón",
        string authorizationCode { get; }//: "080818",
        string shippingFirstName { get; }//: "Andrés",
        string authorizationResult { get; }//: "080818",
        string? descriptionProducts { get; }//: null,
        string paymentReferenceCode { get; }//: "2584",
        string? purchaseCurrencyCode { get; }//: null,
        string? purchaseVerification { get; }//: null,
        string purchaseOperationNumber { get; }//: "0000014279-020203.001",
        int storeId { get; }//: 1083,
        DateTransaction? dateTransaction { get; }//: Object
        string? type { get; }//: "pickup",
        int orderId { get; }//: 10128763
        string? referenceNumber { get; }
        string? loyaltyOps { get; }
        string? loyaltyMarketing { get; }
        string? loyaltyAmount { get; }
        string? loyaltyPurchaseAmount { get; }
    }

    public class DateTransaction
    {
        public string? date { get; set; } //: "2023-10-30 08:08:19",
        public int timezone_type { get; set; } //: 3,
        public string? timezone { get; set; } //: "America/Guayaquil",
        public int weekNumber { get; set; } //: 5
    }
}