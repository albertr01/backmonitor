using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using WebApplication1.Controllers;
using WebApplication1.Models.DB;
using WebApplication1.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;
using WebApplication1.Connections.BD;
using WebApplication1.Models.Request;
using WebApplication1.Models.Response.Local.Salida;
using Microsoft.AspNetCore.Http;
using Serilog;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;


namespace WebApplication1.Connections.BD
{
    public class ListaRestrictivaMetodos(MonitoreopyaContext context, ILogger<ListaRestrictivaMetodos> logger, IConfiguration configuration)
    {
        private readonly MonitoreopyaContext _context = context;
        private readonly ILogger<ListaRestrictivaMetodos> _logger = logger;
        private readonly IConfiguration _configuration = configuration;

        public class ListaRestrictivaResponse
        {
            public object? ExternalResultOFAC { get; set; }

            public object? ExternalResultONU { get; set; }

            public IEnumerable<ListaPep>? InternalResultsPEP { get; set; }
        }

        public async Task<ListaRestrictivaResponse> ConsoltaListaRestrictiva(int? documento, string? name, int? tipoBusqueda)
        {
            try
            {
                string[] nombCompleto;
                // Determinar si el valor es un nombre o una cédula

                // Variables para almacenar los resultados
                object? externalResultOFAC = null;
                object? externalResultONU = null;
                IEnumerable<ListaPep>? internalResultsPEP = null;

                if (tipoBusqueda == 2 || tipoBusqueda == 3)
                {
                    // Leer los valores de OFAC y ONU desde appsettings.json
                    string? ofacUrl = _configuration["APIListaRestrictiva:OFAC"];

                    if (string.IsNullOrEmpty(name))
                    {
                        throw new ArgumentNullException(nameof(name), "El parámetro name no puede ser nulo o vacío.");
                    }

                    nombCompleto = name.Split(' ');

                    string? onuUrl = _configuration["APIListaRestrictiva:ONU"];

                    if (string.IsNullOrEmpty(ofacUrl) || string.IsNullOrEmpty(onuUrl))
                    {
                        throw new InvalidOperationException("OFAC or ONU URL no configurado en appsettings.json.");
                    }

                    _logger.LogInformation("OFAC URL: {OfacUrl}", ofacUrl);
                    _logger.LogInformation("ONU URL: {OnuUrl}", onuUrl);


                    using var httpClient = new HttpClient();

                    // Construir la URL con los parámetros
                    var requestUrlOFAC = $"{ofacUrl}?firstName={nombCompleto[0]}&lastName={nombCompleto[1]}";

                    var requestUrlONU = $"{onuUrl}?firstName={nombCompleto[0]}&lastName={nombCompleto[1]}";

                    // Realizar la solicitud GET al servicio externo
                    var responseOFAC = await httpClient.GetAsync(requestUrlOFAC);
                    var responseONU = await httpClient.GetAsync(requestUrlONU);

                    if (responseOFAC.IsSuccessStatusCode)
                    {
                        var contentOFAC = await responseOFAC.Content.ReadAsStringAsync();
                        externalResultOFAC = JsonSerializer.Deserialize<object>(contentOFAC);
                    }
                    else
                    {
                        _logger.LogError("Error al consultar el servicio externo OFAC: {StatusCode}", responseOFAC.StatusCode);
                    }

                    if (responseONU.IsSuccessStatusCode)
                    {
                        var contentONU = await responseONU.Content.ReadAsStringAsync();
                        externalResultONU = JsonSerializer.Deserialize<object>(contentONU);
                    }
                    else
                    {
                        _logger.LogError("Error al consultar el servicio externo ONU: {StatusCode}", responseONU.StatusCode);
                    }

                }

                if (tipoBusqueda == 1 || tipoBusqueda == 3)
                {
                    // Consultar la base de datos
                    var queryPEP = _context.ListaPeps.AsQueryable() ?? throw new BadRequestException("No se encontró la propiedad en el contexto");


                    if (documento != null)
                    {
                        // Buscar por cédula
                        queryPEP = queryPEP.Where(list => list.NumDocumento == documento);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(name))
                        {
                            queryPEP = queryPEP.Where(list => list.NombApellido.Contains(name));
                        }
                        else
                        {
                            throw new ArgumentNullException(nameof(name), "El parámetro name no puede ser nulo o vacío.");
                        }
                        // Buscar por nombre
                        queryPEP = queryPEP.Where(list => list.NombApellido.Contains(name));
                    }

                    //var totalRegistrosPEP = await queryPEP.CountAsync();

                    internalResultsPEP = [.. queryPEP.OrderByDescending(list => list.Id)];

                }
                // Retornar los resultados combinados
                return new ListaRestrictivaResponse
                {
                    ExternalResultOFAC = externalResultOFAC,
                    ExternalResultONU = externalResultONU,
                    InternalResultsPEP = internalResultsPEP,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar el servicio externo");
                throw; // Lanza la excepción para que el controlador la maneje
            }
        }
    }
}