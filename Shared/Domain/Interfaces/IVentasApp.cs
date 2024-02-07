using Trade.Domain.Entities;
using Trade.Domain.Interfaces;

namespace Shared.Domain.Interfaces
{
    public interface IVentasApp
    {
        string? Bin { get; }
        string? Tarjeta { get; }
        string? Adicional1 { get; }
        string? Idacquirer { get; }
        string? IdCommerce { get; }
        string? Tipo { get; }
        string? Banco { get; }
        string? Codzip { get; }
        string? Mensaje { get; }
        string? IdTransaccion { get; }
        string? Correo { get; }
        decimal? Valor { get; }
        string? Direccion { get; }
        string? Pais { get; }
        string? Apellido { get; }
        string? Cod_Autorizacion { get; }
        string? Nombre { get; }
        string? CodReferenciaPago { get; }
        string? NumeroCompra { get; }
        int? CodTienda { get; }
        DateTime? Fecha { get; }
        string? CodError { get; }
        string? Hora { get; }
        string? Voucher { get; }
        int? CuentaId { get; }
        string? CuentaNombre { get; }
        double? Subtotal { get; }
        double? Descuento { get; }
        double? Iva { get; }
        double? IvaAplicado { get; }
        double? Comision { get; }
        double? FidelizacionOpera { get; }
        double? FidelizacionMerca { get; }
        double? FidelizacionTotal { get; }
        double? FidelizacionValor { get; }
    }
}