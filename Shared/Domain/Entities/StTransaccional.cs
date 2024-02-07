using System;

namespace Shared.Domain.Entities

{
    public class StTransaccional
    {
        public string Merchantid { get; set; }
        public DateTime Fecha_Transaccion { get; set; }
        public string Hora_Transaccion { get; set; }
        public char Estado { get; set; }
        public string Numero_Lote { get; set; }
        public decimal Face_Value { get; set; }
        public string Id_Grupo_Tarjeta { get; set; }
        public string Id_Adquirente { get; set; }
        public string numero_tarjeta_mask { get; set; }
        public string Numero_Autorizacion { get; set; }
        public string Numero_Referencia { get; set; }
        public string Tipo_Transaccion { get; set; }
        public string Resultado_Externo { get; set; }
        public int Tipo_Switch { get; set; }
        public int origen_Transaccion { get; set; }
        public string Sistema { get; set; }
        public string Voucher { get; set; }
        public string CuentaNombre { get; set; }
        public float Subtotal { get; set; }
        public float Descuento { get; set; }
        public float Iva { get; set; }
        public float IvaAplicado { get; set; }
        public float FidelizacionOpera { get; set; }
        public float FidelizacionMerca { get; set; }
        public float FidelizacionTotal { get; set; }
        public float FidelizacionValor { get; set; }

        public static List<StTransaccional> Where(Dictionary<string, string> condiciones)
        {
            List<StTransaccional> resultados = new List<StTransaccional>();

            foreach (var instancia in ObtenerTodasLasInstancias())
            {
                bool cumpleCondiciones = true;

                foreach (var condicion in condiciones)
                {
                    if (
                        !string.Equals(
                            GetPropertyValue(instancia, condicion.Key),
                            condicion.Value,
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                    {
                        cumpleCondiciones = false;
                        break;
                    }
                }

                if (cumpleCondiciones)
                {
                    resultados.Add(instancia);
                }
            }

            return resultados;
        }

        public static void Update(Dictionary<string, string> nuevoRegistro)
        {
            //Lógica para actualizar el registro
            Console.WriteLine("Registro actualizado");
        }

        public static void Insert(Dictionary<string, string> nuevoRegistro)
        {
            //Lógica para insertar un nuevo registro
            Console.WriteLine("Nuevo registro insertado");
        }

        private static IEnumerable<StTransaccional> ObtenerTodasLasInstancias()
        {
            //Lógica para obtener todas las instancias de la clase
            return new List<StTransaccional>();
        }

        private static string GetPropertyValue(StTransaccional instancia, string propertyName)
        {
            // Este método obtiene el valor de una propiedad de una instancia de la clase
            // Necesitarás implementar la lógica real para obtener el valor de la propiedad
            // Este es un ejemplo simple, necesitarás adaptarlo a tus necesidades reales
            switch (propertyName)
            {
                case "Merchantid":
                    return instancia.Merchantid;
                case "Fecha_Transaccion":
                    return instancia.Fecha_Transaccion.ToString();
                default:
                    return null;
            }
        }
    }
}
