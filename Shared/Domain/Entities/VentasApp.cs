
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Domain.Interfaces;

namespace Shared.Domain.Entities
{
    public abstract class VentasApp : IVentasApp
    {
        public string? Bin { get; set; } = "";
        public string? Tarjeta { get; set; } = "";
        public string? Adicional1 { get; set; } = "";
        public string? Idacquirer { get; set; } = "";
        public string? IdCommerce { get; set; } = "";
        public string? Tipo { get; set; } = "";
        public string? Banco { get; set; } = "";
        public string? Codzip { get; set; } = "";
        public string? Mensaje { get; set; } = "";
        public string? IdTransaccion { get; set; } = "";
        public string? Correo { get; set; } = "";
        public decimal? Valor { get; set; } = 0;
        public string? Direccion { get; set; } = "";
        public string? Pais { get; set; } = "";
        public string? Apellido { get; set; } = "";
        [System.ComponentModel.DataAnnotations.Key]
        public string? Cod_Autorizacion { get; set; } = "";
        public string? Nombre { get; set; } = "";
        public string? CodReferenciaPago { get; set; } = "";
        public string? NumeroCompra { get; set; } = "";
        public int? CodTienda { get; set; } = 0;
        public DateTime? Fecha { get; set; } = DateTime.Now;
        public string? CodError { get; set; } = "";
        public string? Hora { get; set; } = "";
        public string? Voucher { get; set; } = "";
        public int? CuentaId { get; set; } = 0;
        public string? CuentaNombre { get; set; } = "";
        public double? Subtotal { get; set; } = 0;
        public double? Descuento { get; set; } = 0;
        public double? Iva { get; set; } = 0;
        public double? IvaAplicado { get; set; } = 0;
        public double? Comision { get; set; } = 0;
        public double? FidelizacionOpera { get; set; } = 0;
        public double? FidelizacionMerca { get; set; } = 0;
        public double? FidelizacionTotal { get; set; } = 0;
        public double? FidelizacionValor { get; set; } = 0;
    }
}
