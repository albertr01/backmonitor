using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using WebApplication1.Models.Response.Local.Salida;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    /// <summary>
    /// Servicios para seleccionables
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SeleccionablesController : ControllerBase
    {
        /// <summary>
        /// Servicio para obtener seleccionables
        /// </summary>
        /// <param name="nombreSeleccionable">Nombre del seleccionable. Posibles valores: tipoIdentificacion, tipoCuenta y tipoMoneda</param>
        /// <returns><see cref="TablaSalida"/></returns>
        [HttpGet]
        [SwaggerResponse(200, type: typeof(TablaSalida))]
        public IActionResult ObtenerSeleccionable(string nombreSeleccionable)
        {
            switch(nombreSeleccionable)
            {
                case "tipoIdentificacion":
                {
                    return Ok(new List<TablaSalida>()
                    {
                        new TablaSalida
                        {
                            Id = 1,
                            Codigo = "TIV",
                            Nombre = "Venezolana"
                        },
                        new TablaSalida
                        {
                            Id = 2,
                            Codigo = "TIE",
                            Nombre = "Extranjero"
                        },
                        new TablaSalida
                        {
                            Id = 3,
                            Codigo = "TIP",
                            Nombre = "Pasaporte"
                        },
                        new TablaSalida
                        {
                            Id = 4,
                            Codigo = "TIJ",
                            Nombre = "Juridico"
                        },
                        new TablaSalida
                        {
                            Id = 5,
                            Codigo = "TIG",
                            Nombre = "Gubernamental"
                        },
                        new TablaSalida
                        {
                            Id = 6,
                            Codigo = "TIC",
                            Nombre = "Comuna"
                        },
                        new TablaSalida
                        {
                            Id = 7,
                            Codigo = "TIF",
                            Nombre = "Firma Personal"
                        }
                    });
                }
                case "tipoCuenta":
                {
                    return Ok(new List<TablaSalida>()
                    {
                        new TablaSalida
                        {
                            Id = 1,
                            Codigo = "TCA",
                            Nombre = "Ahorros"
                        },
                        new TablaSalida
                        {
                            Id = 2,
                            Codigo = "TCCO",
                            Nombre = "Corriente"
                        },
                        new TablaSalida
                        {
                            Id = 3,
                            Codigo = "TCCR",
                            Nombre = "Crédito"
                        }
                    });
                }
                case "tipoMoneda":
                {
                    return Ok(new List<TablaSalida>()
                    {
                        new TablaSalida
                        {
                            Id = 1,
                            Codigo = "VEF",
                            Nombre = "VEF"
                        },
                        new TablaSalida
                        {
                            Id = 2,
                            Codigo = "USD",
                            Nombre = "USD"
                        },
                        new TablaSalida
                        {
                            Id = 3,
                            Codigo = "EUR",
                            Nombre = "EUR"
                        }
                    });
                }
                case "tipoDiligencia":
                    {
                        return Ok(new List<TablaSalida>()
                        {
                            new TablaSalida
                            {
                                Id = 1,
                                Codigo = "RA",
                                Nombre = "Riesgo Alto"
                            },
                            new TablaSalida
                            {
                                Id = 2,
                                Codigo = "RM",
                                Nombre = "Riesgo Moderado"
                            },
                            new TablaSalida
                            {
                                Id = 3,
                                Codigo = "RB",
                                Nombre = "Riesgo Bajo"
                            }
                        });
                    }
                default:
                {
                        return Ok(new List<TablaSalida>());
                }
            }
        }
    }
}
