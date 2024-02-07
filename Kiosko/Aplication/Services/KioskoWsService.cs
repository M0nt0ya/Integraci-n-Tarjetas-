using System.Net.Http.Headers;
using Kiosko.Aplication.Util;
using Kiosko.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared.Domain.Entities;

namespace Kiosko.Aplication.Services
{
    public class KioskoWsService
    {
        public KioskoApp Kiosko { get; private set; }
        private string merchantId;
        private string mensajeError;
        public string NombreProceso = "IMPORTAR_VENTAS_Kiosko";
        private readonly ILogger logger;

        public KioskoWsService(ILogger logger, KioskoApp kiosko)
        {
            this.logger = logger;
            Kiosko = kiosko;
            merchantId = null;
            mensajeError = string.Empty;
        }

        public void ImportarVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            string ventas = ConsultarVentas(fechaInicio, fechaFin);

            if (string.IsNullOrEmpty(ventas))
            {
                Console.WriteLine($"Error al importar ventas. {mensajeError}");
                // Puedes manejar la notificación o el registro de errores aquí.
                return;
            }

            JArray objVentas = JArray.Parse(ventas);
            List<JToken> ventasError = new List<JToken>();

            foreach (JToken ventaToken in objVentas)
            {
                var venta = ventaToken.ToObject<Dictionary<string, string>>();

                if (venta == null)
                {
                    ventasError.Add(ventaToken);
                    continue;
                }

                bool resultadoInsercion = InsertarVenta(venta);

                if (!resultadoInsercion)
                {
                    ventasError.Add(ventaToken);
                }
            }

            if (ventasError.Count > 0)
            {
                Console.WriteLine(
                    $"Error al insertar algunas ventas. {JsonConvert.SerializeObject(ventasError)}"
                );
                // Puedes manejar la notificación o el registro de errores aquí.
            }
        }

        public string ConsultarVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            string apiToken = ObtenerTokenAPI();

            // TODO: En lugar de establecer en falso, disparar el envío de una notificación.
            if (string.IsNullOrEmpty(apiToken))
            {
                Console.WriteLine("No hay token en la petición");
                // Puedes manejar la notificación o el registro de errores aquí.
                return null;
            }

            string url = $"{Kiosko.URLServidor()}{KioskoApp.ENDPOINT_CONSULTAR_VENTAS}";

            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using (HttpClient clienteHttp = new HttpClient(httpClientHandler))
            {
                clienteHttp.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
                clienteHttp.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

                Dictionary<string, string> parametrosQuery = new Dictionary<string, string>
                {
                    { "fechainicio", fechaInicio.ToString("yyyy-MM-dd") },
                    { "fechafin", fechaFin.ToString("yyyy-MM-dd") }
                };

                Console.WriteLine("opcionesClienteHttp");
                Console.WriteLine(
                    $"parametrosQuery: {JsonConvert.SerializeObject(parametrosQuery)}"
                );

                try
                {
                    var response = clienteHttp
                        .GetAsync(url + ToQueryString(parametrosQuery))
                        .Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(
                            $"Error al consumir endpoint ({url}): HTTP {response.StatusCode}, {response.Content.ReadAsStringAsync().Result}"
                        );
                        return null;
                    }

                    string contenidoRespuesta = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine($"Ventas Kiosko {Kiosko.Direccion}: {contenidoRespuesta}");

                    return contenidoRespuesta;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Error al consumir endpoint ({url}): {ex.Message}");
                    // Puedes manejar la notificación o el registro de errores aquí.
                    return null;
                }
            }
        }

        //Convierte los parámetros del diccionario en una cadena de consulta
        private string ToQueryString(Dictionary<string, string> parameters)
        {
            if (parameters == null || parameters.Count == 0)
            {
                return string.Empty;
            }

            var queryString = string.Join(
                "&",
                parameters.Select(kvp =>
                    $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"
                )
            );
            return $"?{queryString}";
        }

        public string ObtenerTokenAPI()
        {
            if (Kiosko.TokenValido())
            {
                return Kiosko.TokenAcceso;
            }

            return CrearResetearToken();
        }

        public string CrearResetearToken()
        {
            string datosNuevoToken = ConsultarEndpointToken();

            if (string.IsNullOrEmpty(datosNuevoToken))
            {
                Console.WriteLine("No hay token en la petición");
                return null;
            }

            dynamic objDatosNuevoToken = JsonConvert.DeserializeObject(datosNuevoToken);

            if (objDatosNuevoToken.codigo != "200")
            {
                Console.WriteLine(objDatosNuevoToken.codigo);
                return null;
            }

            dynamic data = objDatosNuevoToken.data;
            string accessToken = data.api_token;
            Dictionary<string, object> decodedToken =
                (Dictionary<string, object>)Helpers.DecodeJWT(accessToken);
            Dictionary<string, object> tokenClaims =
                (Dictionary<string, object>)decodedToken["claims"];

            DateTime fechaCreado = DateTimeOffset
                .FromUnixTimeSeconds((long)tokenClaims["iat"])
                .DateTime;
            DateTime fechaCaduca = DateTimeOffset
                .FromUnixTimeSeconds((long)tokenClaims["exp"])
                .DateTime;

            Kiosko.TokenFechaCreado = fechaCreado;
            Kiosko.TokenFechaCaduca = fechaCaduca;
            Kiosko.TokenAcceso = accessToken;

            Kiosko.Save();

            return accessToken;
        }

        private string ConsultarEndpointToken()
        {
            var clienteHttp = new HttpClient();
            var url = this.Kiosko.CrearURL(KioskoApp.ENDPOINT_TOKEN);

            try
            {
                var content = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("email", this.Kiosko.Email),
                        new KeyValuePair<string, string>("password", this.Kiosko.Clave)
                    }
                );

                var response = clienteHttp.PostAsync(url, content).Result;
                var contenidoRespuesta = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(
                        $"Error al obtener token de Kiosko: HTTP {response.StatusCode}, {contenidoRespuesta}"
                    );
                    return null;
                }

                Console.WriteLine($"Respuesta generar TOKEN: {contenidoRespuesta}");
                return contenidoRespuesta;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error al obtener token de Kiosko: {ex.Message}");
                return null;
            }
        }

        private void InvalidarToken()
        {
            this.Kiosko.TokenAcceso = null;
            this.Kiosko.Save();
        }

        private bool InsertarVenta(Dictionary<string, string> venta)
        {
            if (venta.Count == 0)
            {
                logger.LogError("Venta Kiosko Vacia");
                return false;
            }

            Dictionary<string, string> datosNuevoRegistro = new Dictionary<string, string>
            {
                { "Merchantid", getMerchantId() },
                { "Fecha_Transaccion", venta["Fecha_Transaccion"] },
                { "Hora_Transaccion", FormatearHoraTransaccion(venta["Hora_Transaccion"]) },
                { "Estado", venta["Estado"] },
                { "Numero_Lote", venta["Numero_Lote"] },
                { "Face_Value", venta["Face_Value"] },
                { "Id_Grupo_Tarjeta", FormatearTipoTarjeta(venta["Id_Grupo_Tarjeta"]) },
                { "Id_Adquirente", venta["Id_Adquirente"] },
                { "numero_tarjeta_mask", venta["numero_tarjeta_mask"] },
                { "Numero_Autorizacion", venta["Numero_Autorizacion"] },
                { "Numero_Referencia", venta["Numero_Referencia"] },
                { "Tipo_Transaccion", venta["Tipo_Transaccion"] },
                { "Resultado_Externo", venta["Resultado_externo"] },
                { "Tipo_Switch", venta["Tipo_Switch"] },
                { "origen_Transaccion", venta["Origen_Transaccion"] },
                { "Sistema", "MAXPOINT" }
            };

            if (datosNuevoRegistro.Any(kvp => string.IsNullOrEmpty(kvp.Value)))
            {
                return false;
            }

            try
            {
                var registro = StTransaccional
                    .Where(
                        datosNuevoRegistro
                            .Where(kvp =>
                                new[]
                                {
                                    "Merchantid",
                                    "Fecha_Transaccion",
                                    "Hora_Transaccion",
                                    "Numero_Referencia",
                                    "Numero_Autorizacion",
                                    "numero_tarjeta_mask"
                                }.Contains(kvp.Key)
                            )
                            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                    )
                    .ToList();

                if (registro.Count > 0)
                {
                    var registroActualizado = StTransaccional
                        .Where(
                            new[]
                            {
                                new KeyValuePair<string, string>(
                                    "Merchantid",
                                    datosNuevoRegistro["Merchantid"]
                                ),
                                new KeyValuePair<string, string>(
                                    "Fecha_Transaccion",
                                    datosNuevoRegistro["Fecha_Transaccion"]
                                ),
                                new KeyValuePair<string, string>(
                                    "Hora_Transaccion",
                                    datosNuevoRegistro["Hora_Transaccion"]
                                ),
                                new KeyValuePair<string, string>(
                                    "Numero_Referencia",
                                    datosNuevoRegistro["Numero_Referencia"]
                                ),
                                new KeyValuePair<string, string>(
                                    "numero_tarjeta_mask",
                                    datosNuevoRegistro["numero_tarjeta_mask"]
                                ),
                                new KeyValuePair<string, string>(
                                    "Numero_Autorizacion",
                                    datosNuevoRegistro["Numero_Autorizacion"]
                                )
                            }.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                        )
                        .ToList();

                    if (registroActualizado.Count > 0)
                    {
                        StTransaccional.Update(datosNuevoRegistro);
                        logger.LogInformation("Registro actualizado");
                        return true;
                    }
                }

                StTransaccional.Insert(datosNuevoRegistro);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return false;
            }

            return true;
        }

        private string getMerchantId()
        {
            if (!string.IsNullOrEmpty(this.merchantId))
            {
                return this.merchantId;
            }

            var restaurant = Restaurant.Find(this.Kiosko.IdLocal);
            var merchantId = restaurant.SwitchT;
            this.merchantId = merchantId;
            return merchantId;
        }

        private string FormatearHoraTransaccion(string horaTransaccion)
        {
            var horaTx1 = horaTransaccion.Split('.');
            return horaTx1[0].Replace(":", "");
        }

        private string FormatearTipoTarjeta(string strGrupoTarjeta)
        {
            var grupoTarjeta = strGrupoTarjeta.ToUpper().Trim();

            switch (grupoTarjeta)
            {
                case "DEBITO":
                    return "DEBI";
                case "DINERS CLUB":
                    return "DINE";
                case "MASTERCARD":
                    return "MAST";
                case "DISCOVER":
                    return "DISC";
                case "AMERICAN EXPRESS":
                    return "AMEX";
                case "ALIA":
                    return "CUOT";
                case "UNION PAY":
                    return "UPAY";
                default:
                    return grupoTarjeta;
            }
        }
    }
}
