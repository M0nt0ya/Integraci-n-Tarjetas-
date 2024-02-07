using System.Net.Http.Headers;
using Kiosko.Domain.Entities;
using Newtonsoft.Json;

namespace Kiosko.Infrastructure.ConsultExternalServices
{
    public class KioskoExternalApiService : IKioskoExternalApiService
    {
        private readonly HttpClient _client = new();
        private readonly IConfiguration _configuration;

        public KioskoExternalApiService(IConfiguration configuration)
        {
            _configuration =
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<List<KioskoApp>> GetVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            var response = new HttpResponseMessage();

            try
            {
                fechaInicio =
                    fechaInicio != DateTime.MinValue ? fechaInicio : DateTime.Now.AddDays(-1).Date;
                fechaFin = fechaFin != DateTime.MinValue ? fechaFin : DateTime.Now.Date;

                var apiUrl = $"{GetApiBaseUrl()}/{KioskoApp.ENDPOINT_CONSULTAR_VENTAS}";

                string token = await GetAuthenticationToken();

                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );

                response = await _client.GetAsync(
                    $"{apiUrl}?fechainicio={fechaInicio:yyyy-MM-dd}&fechafin={fechaFin:yyyy-MM-dd}"
                );

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();

                    var Kiosko = JsonConvert.DeserializeObject<List<KioskoApp>>(data);

                    return Kiosko;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in KioskoExternalApiService - GetVentas: {ex.Message}");
                throw;
            }

            return new List<KioskoApp>();
        }

        private async Task<string> GetAuthenticationToken()
        {
            try
            {
                return "dummy_token";
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error in KioskoExternalApiService - GetAuthenticationToken: {ex.Message}"
                );
                throw;
            }
        }

        private string GetApiBaseUrl()
        {
            return _configuration["KioskoSettings:ApiBaseUrl"]
                ?? throw new ArgumentNullException("KioskoSettings:ApiBaseUrl");
        }
    }
}

//Este servicio encapsula la lógica de interacción con la API externa de Kiosko, proporcionando un método para obtener información de ventas
