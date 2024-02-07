using Trade.Domain.Interfaces;

namespace Trade.Domain.Entities
{
    public class TransactionTrade : ITransactionTrade
    {
        public int accountId { get; } //: 2,
        public string accountName { get; set; } = "";//: "Juan Valdez",
        public int vendorExternalId { get; }//: 12,
        public string vendorName{ get; set; } = "";//: "JUAN VALDEZ",
        public string subtotal{ get; set; } = "";//: "18.5714",
        public string discounts{ get; set; } = "";//: "0.0000",
        public string taxes{ get; set; } = "";//: "2.2286",
        public double iva { get; }//: 0.12,
        public double commission { get; }//: 0.09,
        public string bin{ get; set; } = "";//: "46857100",
        public string brand{ get; set; } = "";//: "VISA",
        public int errorCode { get; }//: 0,
        public string? reserved1 { get; }//: null,
        public string acquirerId{ get; set; } = "";//: "Kushki",
        public string? idCommerce { get; }//: null,
        public string reserved22{ get; set; } = "";//: "debit",
        public string? reserved23 { get; }//: null,
        public string? shippingZIP { get; }//: null,
        public string errorMessage{ get; set; } = "";//: "Approved",
        public string IDTransaction{ get; set; } = "";//: "128233033289923451",
        public string shippingEmail{ get; set; } = "";//: "stalincalderon2015@gmail.com",
        public decimal purchaseAmount { get; }//: 20.8,
        public string shippingAddress{ get; set; } = "";//: "  ",
        public string? shippingCountry { get; }//: null,
        public string shippingLastName{ get; set; } = "";//: "Calderón",
        public string authorizationCode{ get; set; } = "";//: "080818",
        public string shippingFirstName{ get; set; } = "";//: "Andrés",
        public string authorizationResult{ get; set; } = "";//: "080818",
        public string? descriptionProducts { get; }//: null,
        public string paymentReferenceCode{ get; set; } = "";//: "2584",
        public string? purchaseCurrencyCode { get; }//: null,
        public string? purchaseVerification { get; }//: null,
        public string purchaseOperationNumber{ get; set; } = "";//: "0000014279-020203.001",
        public int storeId { get; }//: 1083,
        public DateTransaction? dateTransaction { get; set; }//: Object
        public string? type { get; set; } = "";//: "pickup",
        public int orderId { get; }//: 10128763
        public string? referenceNumber { get; }
        public string? loyaltyOps { get; }
        public string? loyaltyMarketing { get; }
        public string? loyaltyAmount { get; }
        public string? loyaltyPurchaseAmount { get; }
    }
}