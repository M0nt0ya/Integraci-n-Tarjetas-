using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Trade.Domain.Entities;
using Trade.Domain.Interfaces;

public class ExternalApiService(IConfiguration configuration) : IExternalApiService
{
    private readonly HttpClient _client = new();
    private readonly RedisControllerRepository _redis = new RedisControllerRepository(configuration);
    private readonly string urlApiService = configuration["TradeSettings:urlApiService"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string endpointInfoPayments = configuration["TradeSettings:endpointInfoPayments"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string oauthEndpointToken = configuration["TradeSettings:oauth_endpoint_token"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string oauthClientId = configuration["TradeSettings:oauth_client_id"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string oauthClientSecret = configuration["TradeSettings:oauth_client_secret"] ?? throw new ArgumentNullException(nameof(configuration));
    private readonly string oauthGrantType = configuration["TradeSettings:oauth_grant_type"] ?? throw new ArgumentNullException(nameof(configuration));

    public async Task<List<TransactionTrade>> GetTransactions(string? fechaInicio = null, string? fechaFin = null)
    {
        var response = new HttpResponseMessage();
        //response.StatusCode;
        try
        {
            // Si fechaInicio o fechaFin son null, asigna los valores predeterminados
            fechaInicio ??= DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            fechaFin ??= DateTime.Now.ToString("yyyy-MM-dd");

            var uri = $"{urlApiService + endpointInfoPayments}?country=ecuador&start_date={fechaInicio}&end_date={fechaFin}";
            //Console.WriteLine(uri);
            // Aquí es donde obtienes tu token de autenticación
            string token = await GetAuthenticationToken();

            // Añade el token a las cabeceras de la petición
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            response = await _client.GetAsync(uri);

            //Console.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeAnonymousType(
                    data,
                    new
                    {
                        code = "",
                        status = "",
                        message = "",
                        data = new List<TransactionTrade>()
                    }
                    );
                
                if (apiResponse?.data != null)
                {
                    return apiResponse.data;
                }
            }
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Error file ExternalApiService method GetTransactions: " + ex.Message);
            //throw;
            throw new Exception($"Error GetTransactions: {response.StatusCode}");
        }
        return [];
    }

    private async Task<string> GetAuthenticationToken()
    {
        try
        {
            // Intenta obtener el token de Redis
            string token = _redis.GetData("TRADE_API_TOKEN");

            // Si el token no está en Redis, solicítalo al cliente
            if (string.IsNullOrEmpty(token))
            {
                var tokenRequest = new HttpRequestMessage(HttpMethod.Post, $"{urlApiService + oauthEndpointToken}")
                {
                    Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    {"grant_type", oauthGrantType},
                    {"client_id", oauthClientId},
                    {"client_secret", oauthClientSecret}
                })
                };

                var tokenResponse = await _client.SendAsync(tokenRequest);

                if (tokenResponse.IsSuccessStatusCode)
                {
                    var responseData = await tokenResponse.Content.ReadAsStringAsync();
                    var tokenData = JsonConvert.DeserializeObject<dynamic>(responseData);
                    if (tokenData != null)
                    {
                        token = tokenData.access_token;

                        // Guarda el token en Redis para su uso futuro
                        _redis.SetData("TRADE_API_TOKEN", token);
                    }
                }
                else
                {
                    throw new Exception("Failed to fetch authentication token.");
                }
            }

            return token;
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Error GetAuthenticationToken: " + ex.Message);
            throw;
        }
    }

}



// using System.IdentityModel.Tokens.Jwt;
// using System.Net;
// using System.Net.Http.Headers;
// using System.Text.Json.Serialization;
// using Trade.Domain.Interfaces;
// using Trade.Infrastructure.Interface;

// namespace Trade.Infrastructure.ConsultExternalServices
// {
//     public class ExternalApiService(IConfiguration configuration)
//     {
//         private readonly string urlApiService = configuration["TradeSettings:urlApiService"] ?? throw new ArgumentNullException(nameof(configuration));
//         private readonly string endpointInfoPayments = configuration["TradeSettings:endpointInfoPayments"] ?? throw new ArgumentNullException(nameof(configuration));
//         private readonly string oauthClientId = configuration["TradeSettings:oauth_client_id"] ?? throw new ArgumentNullException(nameof(configuration));
//         private readonly string oauthClientSecret = configuration["TradeSettings:oauth_client_secret"] ?? throw new ArgumentNullException(nameof(configuration));
//         private readonly string oauthGrantType = configuration["TradeSettings:oauth_grant_type"] ?? throw new ArgumentNullException(nameof(configuration));
//         public Task<List<ITransactionTrade>> GetTransactions(DateTime? fechaInicio = null, DateTime? fechaFin = null)
//         {
//             string servicio = "TRADE";
//             string verboHttpPeticion = "GET";
//             DateTime fechaActual = DateTime.Now;

//             if (fechaInicio == null)
//             {
//                 if (fechaActual.Hour == 0)
//                 {
//                     fechaInicio = fechaActual.AddDays(-1).Date;
//                 }
//                 else
//                 {
//                     fechaInicio = fechaActual.Date;
//                 }
//             }

//             if (fechaFin == null)
//             {
//                 fechaFin = fechaActual.Date;
//             }

//             var parametrosQuery = new Dictionary<string, string>
//         {
//             { "start_date", fechaInicio.Value.ToString("yyyy-MM-dd") },
//             { "end_date", fechaFin.Value.ToString("yyyy-MM-dd") },
//             { "country", "ecuador" },
//             { "account", "" }
//         };

//             string? baseUri = UrlServidor();
//             string url = baseUri + endpointInfoPayments;

//             string apiToken = GetTokenApi();

//             var opcionesClienteHttp = new HttpClient
//             {
//                 BaseAddress = new Uri(baseUri)
//             };
//             opcionesClienteHttp.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);

//             try
//             {
//                 var response = await opcionesClienteHttp.GetAsync(endpoint + "?" + string.Join("&", parametrosQuery.Select(x => $"{x.Key}={x.Value}")));

//                 if (response.StatusCode == HttpStatusCode.Unauthorized)
//                 {
//                     //InvalidarToken();
//                     throw new Exception($"Error al consumir endpoint ({url}): ERROR 401 Unauthorized, revisar los parámetros de autenticación configurados");
//                 }

//                 if ((int)response.StatusCode >= 300)
//                 {
//                     throw new Exception($"Error al consumir endpoint ({url}): \n {await response.Content.ReadAsStringAsync()}");
//                 }

//                 string respuesta = await response.Content.ReadAsStringAsync();

//                 // Escribe en el log solo una vez cada hora
//                 if (DateTime.Now.Minute == 0)
//                 {
//                     Console.WriteLine($"Respuesta recibida: {respuesta}");
//                 }

//                 return respuesta;
//             }
//             catch (Exception ex)
//             {
//                 // Aquí puedes manejar el error como prefieras
//                 throw new Exception($"Error al consumir endpoint del catch ({url}): " + ex.Message);
//             }
//         }

//         private string UrlServidor()
//         {
//             if (string.IsNullOrEmpty(urlApiService))
//             {
//                 throw new Exception("No se ha configurado un servidor para la API de integración");
//                 // Aquí necesitarías implementar el envío de correo electrónico en C#
//                 // MailController correos = new MailController();
//                 // correos.Send("No se ha configurado un servidor para la API de integración", servicio);
//             }

//             return urlApiService;
//         }

//         private string GetTokenApi()
//         {
//             RedisControllerRepository repository = new();
//             string tokenAPI = repository.GetData("TRADE_API_TOKEN");
//             Console.WriteLine(tokenAPI);
//             if (tokenAPI == null)
//             {
//                 return RefreshApiToken();
//             }
//             return tokenAPI;
//         }

//         private string RefreshApiToken()
//         {
//             string service = "TRADE";
//             //MailController mails = new MailController();

//             string baseUri = urlApiService;
//             string endpoint = endpointInfoPayments;
//             string url = baseUri + endpoint;

//             var requestData = new Dictionary<string, string>
//     {
//         {"client_id", oauthClientId},
//         {"client_secret", oauthClientSecret},
//         {"grant_type", oauthGrantType}
//     };

//             HttpClient client = new HttpClient();
//             client.BaseAddress = new Uri(baseUri);
//             client.DefaultRequestHeaders.Accept.Clear();
//             client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//             client.DefaultRequestHeaders.Add("Content-Encoding", "gzip");

//             HttpResponseMessage response;
//             try
//             {
//                 response = client.PostAsync(endpoint, new FormUrlEncodedContent(requestData)).Result;
//             }
//             catch (HttpRequestException ex)
//             {
//                 Console.WriteLine($"RequestException: {ex.Message}");
//                 //mails.Send(ex, service);
//                 throw;
//             }

//             if (!response.IsSuccessStatusCode)
//             {
//                 Console.WriteLine($"Error: {response.StatusCode}");
//                 //mails.Send($"Error: {response.StatusCode}", service);
//                 return null;
//             }

//             string responseContent = response.Content.ReadAsStringAsync().Result;

//             if (!IsJson(responseContent))
//             {
//                 Console.WriteLine("Not JSON: ", responseContent);
//                 //mails.Send(responseContent, service);
//                 return null;
//             }

//             dynamic parsedResponseContent = JsonConverter.DeserializeObject(responseContent);

//             string accessToken = parsedResponseContent.access_token;

//             var jwtToken = new JwtSecurityToken(accessToken);
//             var exp = jwtToken.Claims.First(c => c.Type == "exp").Value;
//             var ttl = int.Parse(exp) - 60 - DateTimeOffset.UtcNow.ToUnixTimeSeconds();

//             // Redis in C# is usually handled with a library like StackExchange.Redis
//             // var redis = ConnectionMultiplexer.Connect("localhost");
//             // IDatabase db = redis.GetDatabase();
//             // db.StringSet("TRADE_API_TOKEN", accessToken);
//             // db.KeyExpire("TRADE_API_TOKEN", TimeSpan.FromSeconds(ttl));

//             return accessToken;
//         }


//     }

// }