
using System.Globalization;
using System.Text.RegularExpressions;
using Shared.Domain.Entities;

namespace Trade.Domain.Entities
{
    public class VentasAppFromTrade : VentasApp
    {
        public VentasAppFromTrade ConvertTransactionTradeToVentasApp(TransactionTrade transactionTrade)
        {
            Console.WriteLine("valor: " + transactionTrade.purchaseAmount);
            var VentasAppFromTrade = new VentasAppFromTrade
            {
                Bin = transactionTrade.bin,// => isset($venta->bin) ? $venta->bin : null,
                Tarjeta = ValidateBrand(transactionTrade.brand),// => $brand,
                Adicional1 = transactionTrade?.reserved1 ?? null,// => isset($venta->reserved1) ? $venta->reserved1 : null,
                Idacquirer = ValidateAcquirerID(transactionTrade?.acquirerId),// => $Idacquirer,
                IdCommerce = transactionTrade?.idCommerce ?? null,// => isset($venta->idCommerce) ? $venta->idCommerce : null,
                Tipo = transactionTrade?.reserved22 ?? null,// => $tipoFormateado,
                Banco = transactionTrade?.reserved23 ?? null,// => isset($venta->reserved23) ? $venta->reserved23 : null,
                Codzip = transactionTrade?.shippingZIP ?? null,// => isset($venta->shippingZIP) ? $venta->shippingZIP : null,
                Mensaje = transactionTrade?.errorMessage ?? null,// => isset($venta->errorMessage) ? $venta->errorMessage : null,
                IdTransaccion = transactionTrade?.IDTransaction ?? null,// => isset($venta->IDTransaction) ? $venta->IDTransaction : null,
                Correo = transactionTrade?.shippingEmail ?? null,// => isset($venta->shippingEmail) ? $venta->shippingEmail : null,
                Valor = transactionTrade?.purchaseAmount ?? null,// => isset($venta->purchaseAmount) ? $venta->purchaseAmount : null,
                Direccion = FormatAddress(transactionTrade?.shippingAddress),// => $direccionFormateada,
                Pais = transactionTrade?.shippingCountry ?? null,// => isset($venta->shippingCountry) ? $venta->shippingCountry : null,
                Apellido = transactionTrade?.shippingLastName ?? null,// => isset($venta->shippingLastName) ? $venta->shippingLastName : null,
                NumeroCompra = transactionTrade?.purchaseOperationNumber ?? null// => isset($venta->purchaseOperationNumber) ? $venta->purchaseOperationNumber : null,
            };
            // propiedades parseadas
            VentasAppFromTrade.Cod_Autorizacion = ParseAuthorizationCode(VentasAppFromTrade.NumeroCompra, transactionTrade?.authorizationCode, VentasAppFromTrade.Idacquirer);// => $authorizationCode,
            VentasAppFromTrade.Nombre = transactionTrade?.shippingFirstName ?? null;// => isset($venta->shippingFirstName) ? $venta->shippingFirstName : null,
            VentasAppFromTrade.CodReferenciaPago = transactionTrade?.paymentReferenceCode ?? null;// => isset($venta->paymentReferenceCode) ? $venta->paymentReferenceCode : null,
            VentasAppFromTrade.CodTienda = transactionTrade?.storeId ?? null;// => isset($venta->storeId) ? $venta->storeId : null,
            VentasAppFromTrade.Fecha = (DateTime?)FormattedStringDate(transactionTrade?.dateTransaction?.date, "fecha");// => $fecha->format("Y-m-d"),
            VentasAppFromTrade.CodError = transactionTrade?.errorCode.ToString() ?? null;// => isset($venta->errorCode) ? '' . $venta->errorCode : null,
            VentasAppFromTrade.Hora = (string?)FormattedStringDate(transactionTrade?.dateTransaction?.date, "hora");// => $fecha->format("His"),
            VentasAppFromTrade.Voucher = transactionTrade?.referenceNumber ?? null;// => isset($venta->referenceNumber) ? $venta->referenceNumber : null,
            VentasAppFromTrade.CuentaId = transactionTrade?.accountId ?? null;// => isset($venta->accountId) ? $venta->accountId : null,
            VentasAppFromTrade.CuentaNombre = transactionTrade?.accountName ?? null;// => isset($venta->accountName) ? $venta->accountName : null,
            VentasAppFromTrade.Subtotal = transactionTrade?.subtotal != null ? GetDoubleFromString(transactionTrade.subtotal) : null;// => isset($venta->Subtotal) ? $venta->Subtotal : null,
            VentasAppFromTrade.Descuento = transactionTrade?.discounts != null ? GetDoubleFromString(transactionTrade.discounts) : null;// => isset($venta->Discounts) ? $venta->Discounts : null,
            VentasAppFromTrade.Iva = transactionTrade?.taxes != null ? GetDoubleFromString(transactionTrade.taxes) : null;// => isset($venta->Taxes) ? $venta->Taxes : null,
            VentasAppFromTrade.IvaAplicado = transactionTrade?.iva != null ? transactionTrade.iva : null;// => isset($venta->iva) ? $venta->iva : null,
            VentasAppFromTrade.Comision = transactionTrade?.commission != null ? transactionTrade.commission : null;// => isset($venta->commission) ? $venta->commission : null,
            VentasAppFromTrade.FidelizacionOpera = transactionTrade?.loyaltyOps != null ? GetDoubleFromString(transactionTrade.loyaltyOps) : null;// => isset($venta->loyaltyOps) ? $venta->loyaltyOps : null,
            VentasAppFromTrade.FidelizacionMerca = transactionTrade?.loyaltyMarketing != null ? GetDoubleFromString(transactionTrade.loyaltyMarketing) : null;// => isset($venta->loyaltyMarketing) ? $venta->loyaltyMarketing : null,
            VentasAppFromTrade.FidelizacionTotal = transactionTrade?.loyaltyAmount != null ? GetDoubleFromString(transactionTrade.loyaltyAmount) : null;// => isset($venta->loyaltyAmount) ? $venta->loyaltyAmount : null,
            VentasAppFromTrade.FidelizacionValor = transactionTrade?.loyaltyPurchaseAmount != null ? GetDoubleFromString(transactionTrade.loyaltyPurchaseAmount) : null;// => isset($venta->loyaltyPurchaseAmount) ? $venta->loyaltyPurchaseAmount : null,

            return VentasAppFromTrade;
        }

        public string ValidateBrand(string? stringBrand)
        {
            string validBrand = (!string.IsNullOrEmpty(stringBrand)) ? stringBrand : "VISA";

            if (stringBrand?.IndexOf("din", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                validBrand = "DINE";
            }

            return validBrand;
        }

        public string? ValidateAcquirerID(string? stringAcquirerID)
        {
            string? validAcquirer = (!string.IsNullOrEmpty(stringAcquirerID)) ? stringAcquirerID : null;

            if (!string.IsNullOrEmpty(validAcquirer) && stringAcquirerID?.IndexOf("Payment", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                validAcquirer = "Paymentez";
            }

            return validAcquirer;
        }

        public string? FormatAddress(string? address)
        {
            string? formattedAddress = null;

            if (!string.IsNullOrEmpty(address) && !string.IsNullOrWhiteSpace(address))
            {
                // Remove non-alphanumeric characters
                formattedAddress = Regex.Replace(address, @"[^a-zA-Z0-9 ]", "");

                // Truncate and add ellipsis if necessary
                if (formattedAddress.Length > 246)
                {
                    formattedAddress = formattedAddress[..246] + "...";
                }
            }

            return formattedAddress;
        }

        public string? ParseAuthorizationCode(string? purchaseOperationNumber, string? authorizationCode, string? IdAcquirer)
        {
            if (purchaseOperationNumber == null)
            {
                return authorizationCode;
            }

            var firtsSegmentPurchaseOperationNumber = purchaseOperationNumber.Split('-')[0];

            if (authorizationCode == null)
            {
                if (IdAcquirer == "Paymentez")
                {
                    var parsedAuthorizationCode = string.Concat("T", firtsSegmentPurchaseOperationNumber.AsSpan(firtsSegmentPurchaseOperationNumber.Length - 5));
                    authorizationCode = parsedAuthorizationCode;
                }
            }

            return authorizationCode;
        }

        public object? FormattedStringDate(string? dateString, string format)
        {
            if (string.IsNullOrEmpty(dateString))
            {
                return null;
            }

            DateTime date = DateTime.ParseExact(dateString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

            if (format == "fecha")
            {
                return date.Date;
            }
            else if (format == "hora")
            {
                return date.ToString("HHmmss");
            }
            else
            {
                return null;
            }
        }

        public double? GetDoubleFromString(string? stringNumber)
        {

            if (double.TryParse(stringNumber, out double doubleNumber))
            {
                return doubleNumber;
            }
            return null;
        }

    }
}
