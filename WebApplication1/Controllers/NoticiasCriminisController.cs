using Google.Apis.Services;
using Google.Apis.CustomSearchAPI.v1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using WebApplication1.Connections.BD;
using WebApplication1.Models.DB;
using WebApplication1.Models.Request;
using WebApplication1.Models.Response.Local.Salida;
using WebApplication1.Utils;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiasCriminisController : ControllerBase
    {
        private readonly ManagementLogs _managementLogs;
        private readonly SubcripcionAlertaMetodos _subcripcionAlertaMetodos;
        private readonly AlertaMetodos _alertaMetodos;
        private readonly ITokenProvider _tokenProvider;
        private readonly IConfiguration _configuration;
        public NoticiasCriminisController(ILogger<GestionSuscripcionAlertasController> logger, MonitoreopyaContext modelContext, 
            ITokenProvider tokenProvider, IConfiguration configuration)
        {
            _managementLogs = new ManagementLogs(logger);
            _subcripcionAlertaMetodos = new(modelContext, logger);
            _alertaMetodos = new(modelContext, logger);
            _tokenProvider = tokenProvider;
            _configuration = configuration;
        }
        /// <summary>
        /// Servicio de registrar alerta
        /// </summary>
        [HttpPost("BuscarPersona")]
        [SwaggerResponse(200, type: typeof(GenericoSalida))]
        public async Task<IActionResult> GuardarSubscripcionAlertaAsync(string busqueda)
        {
            try
            {
                try
                {
                    var searchResults = await SearchGoogleNews(busqueda);

                    if (searchResults != null && searchResults.Items != null && searchResults.Items.Any())
                    {
                        foreach (var item in searchResults.Items)
                        {
                            List<string> palabrasEncontradas;
                            if (KeywordChecker.ContienePalabrasClave(item.Title + " " + item.Snippet, out palabrasEncontradas))
                            {
                                Console.WriteLine($"  -> Palabras clave encontradas: {string.Join(", ", palabrasEncontradas)}");
                            }
                            else
                            {
                                Console.WriteLine("  -> No se encontraron palabras clave relevantes en este snippet.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No se encontraron resultados para la consulta.");
                    }

                    return Ok(new GenericoSalida
                    {
                        Codigo = "0",
                        Descripcion = string.Join(", ", searchResults.Items.Select(i => i.Title).ToList()),
                        DescripcionTécnica = "Exito",
                        Error = false
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al realizar la búsqueda: {ex.Message}");

                    return Ok(new GenericoSalida
                    {
                        Codigo = "1",
                        Descripcion = "Error al realizar búsqueda en internet",
                        DescripcionTécnica = ex.Message,
                        Error = true
                    });
                }
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al realizar búsqueda en internet",
                    data);

                return Ok(new GenericoSalida
                {
                    Codigo = "1",
                    Descripcion = "Error al realizar búsqueda en internet",
                    DescripcionTécnica = e.Message,
                    Error = true
                });
            }
        }

        private async Task<Google.Apis.CustomSearchAPI.v1.Data.Search> SearchGoogleNews(string query)
        {
            var service = new CustomSearchAPIService(new BaseClientService.Initializer()
            {
                ApiKey = _configuration["GoogleSearch:APIKEY"],
                ApplicationName = "Monitor PA" // Un nombre para tu aplicación
            });

            // Crea la solicitud de búsqueda
            var listRequest = service.Cse.List();
            listRequest.Cx = _configuration["GoogleSearch:SEARCH_ENGINE_ID"];
            listRequest.Q = query;
            listRequest.Num = 10; // Número de resultados a obtener (máximo 10 por solicitud)
                                  // Puedes añadir más parámetros como:
                                  // listRequest.Start = 11; // Para paginación (inicia en el resultado 11 para la segunda página)
                                  // listRequest.DateRestrict = "d1"; // Limitar a resultados del último día
                                  // listRequest.Cr = "countryES"; // Limitar a resultados de España
                                  // listRequest.Hl = "es"; // Idioma de los resultados
                                  // listRequest.SiteSearch = "elnacional.com"; // Buscar solo en un sitio específico (si no lo definiste en el CSE)

            // Ejecuta la solicitud
            var searchResult = await listRequest.ExecuteAsync();

            return searchResult;
        }
    }
}
