using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApplication1.Controllers;
using WebApplication1.Models.DB;
using WebApplication1.Models.Response.Local.Salida;
using WebApplication1.Utils;

namespace WebApplication1.Connections.BD
{
    public class SubcripcionAlertaMetodos : Controller
    {
        private readonly MonitoreopyaContext _context;
        private readonly ManagementLogs _managementLogs;

        public SubcripcionAlertaMetodos(MonitoreopyaContext context, ILogger<ControllerBase> logger)
        {
            _context = context;
            _managementLogs = new ManagementLogs(logger);
        }
        /// <summary>
        /// Registrar Alerta
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public Models.DB.SubcripcionAlertum RegistrarSubcripcionAlerta(int? idAlerta, string nombreAlerta, string descripcion, int metododistribucion, int bandejaEntrada, string comentario, bool estado, string usuario, string correo, int? idCliente, 
                bool? notificacionCorreo)
        {
            try
            {
                if(idAlerta == null)
                {
                    var subcripcion = _context.SubcripcionAlerta.Where(a => a.Nombre.ToUpper().Equals(nombreAlerta.ToUpper())).ToList().FirstOrDefault();

                    if (subcripcion != null)
                    {
                        throw new Exception("Alerta ya existente");
                    }
                }
                else
                {
                    var subcripcion = _context.SubcripcionAlerta.Where(a => a.Id == idAlerta && a.IdCliente == idCliente).ToList().FirstOrDefault();
                    if (subcripcion != null)
                    {
                        throw new Exception("Alerta ya existente");
                    }
                }

                var subcripcionAlerta = _context.SubcripcionAlerta.Add(new Models.DB.SubcripcionAlertum
                {
                    BandejaEntrada = idAlerta == null ? null : bandejaEntrada,
                    Comentario = comentario,
                    Descripcion = descripcion,
                    Estado = estado,
                    FechaActualizacion = DateTime.Now,
                    FechaCreacion = DateTime.Now,
                    FkAlerta = idAlerta == null ? null : idAlerta.Value,
                    MetodoDistribucion = idAlerta == null ? null : metododistribucion,
                    Nombre = idAlerta == null ? nombreAlerta : null,
                    Usuario = usuario,
                    Correo = correo,
                    IdCliente = idCliente,
                    NotificacionCorreo = notificacionCorreo
                });

                _context.SaveChanges();

                return subcripcionAlerta.Entity;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    {nameof(idAlerta), idAlerta},
                    {nameof(nombreAlerta), nombreAlerta},
                    {nameof(descripcion), descripcion},
                    {nameof(metododistribucion), metododistribucion},
                    {nameof(bandejaEntrada), bandejaEntrada},
                    {nameof(estado), estado},
                    {nameof(comentario), comentario},
                    {nameof(correo), correo},
                    {nameof(idCliente), idCliente},
                    {nameof(notificacionCorreo), notificacionCorreo},
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Consultar Alerta de montos
        /// </summary>
        /// <returns><see cref="List{AlertaMonto}"/></returns>
        public List<Models.DB.SubcripcionAlertum> ConsultarSuscripcionAlerta()
        {
            try
            {
                var suscripcionAlertas = _context.SubcripcionAlerta.Include(s => s.FkAlertaNavigation).ToList();

                return suscripcionAlertas;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        public List<Models.DB.LogsAuditorium> ConsultarHistorial(int id)
        {
            try
            {
                var historial = _context.LogsAuditoria.Where(a => a.BdTabla == "SubcripcionAlerta").ToList();

                historial = historial.Where(a => a.DetalleRegistro.Split(";")[0].Split(":")[1].Equals(id.ToString())).ToList();

                return historial;
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Modificar Tipo de Alerta
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        public void ModificarSubcripcionAlerta(int id, int? idAlerta, string nombreAlerta, string descripcion, int metododistribucion, int bandejaEntrada, string comentario, bool estado, string usuario, string correo, int? idCliente,
                bool? notificacionCorreo)
        {
            try
            {
                if (idAlerta == null)
                {
                    var subcripcion = _context.SubcripcionAlerta.Where(a => a.Nombre.ToUpper().Equals(nombreAlerta.ToUpper()) && a.Id != id).ToList().FirstOrDefault();

                    if (subcripcion != null)
                    {
                        throw new Exception("Alerta ya existente");
                    }
                }
                else
                {
                    var subcripcion = _context.SubcripcionAlerta.Where(a => a.Id == idAlerta && a.Id != id && a.IdCliente == idCliente).ToList().FirstOrDefault();
                    if (subcripcion != null)
                    {
                        throw new Exception("Alerta ya existente");
                    }
                }

                var alertaSubscripcion = _context.SubcripcionAlerta.FirstOrDefault(x => x.Id == id);

                alertaSubscripcion.BandejaEntrada = bandejaEntrada;
                alertaSubscripcion.Comentario = comentario;
                alertaSubscripcion.Descripcion = descripcion;
                alertaSubscripcion.Estado = estado;
                alertaSubscripcion.FechaActualizacion = DateTime.Now;
                alertaSubscripcion.FkAlerta = idAlerta;
                alertaSubscripcion.MetodoDistribucion = metododistribucion;
                alertaSubscripcion.Nombre = nombreAlerta;
                alertaSubscripcion.Usuario = usuario;
                alertaSubscripcion.NotificacionCorreo = notificacionCorreo;
                alertaSubscripcion.Correo = correo;
                alertaSubscripcion.IdCliente = idCliente;

                _context.SubcripcionAlerta.Update(alertaSubscripcion);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    {nameof(id), id},
                    {nameof(bandejaEntrada), bandejaEntrada},
                    {nameof(comentario), comentario},
                    {nameof(descripcion), descripcion},
                    {nameof(estado), estado},
                    {nameof(idAlerta), idAlerta},
                    {nameof(metododistribucion), metododistribucion},
                    {nameof(nombreAlerta), nombreAlerta},
                    {nameof(usuario), usuario},
                    {nameof(notificacionCorreo), notificacionCorreo},
                    {nameof(correo), correo},
                    {nameof(idCliente), idCliente},
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Cambiar Tipo de Alerta
        /// </summary>
        public void CambiarEstatusSubcripcionAlerta(int id, bool estado, string usuario, string role)
        {
            try
            {

                var alertaSubscripcion = _context.SubcripcionAlerta.FirstOrDefault(x => x.Id == id);

                alertaSubscripcion.Estado = estado;
                alertaSubscripcion.Usuario = usuario;
                alertaSubscripcion.FechaActualizacion = DateTime.Now;

                _context.SubcripcionAlerta.Update(alertaSubscripcion);

                _context.SaveChanges();

                GuardarLog(usuario, role, $"PUT: api/CambiarEstatusSubcripcionAlerta", "SubcripcionAlerta", "APICONTROLLER", $"Registro:{id};Accion:{estado}");
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    {nameof(id), id},
                    {nameof(usuario), usuario},
                    {nameof(estado), estado}
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }
        /// <summary>
        /// Eliminar Tipo de Alerta
        /// </summary>
        public void EliminarSubcripcionAlerta(int id)
        {
            try
            {
                var subscripcionAlerta = _context.SubcripcionAlerta.FirstOrDefault(x => x.Id == id);

                _context.Remove(subscripcionAlerta);

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Dictionary<string, object> data = new()
                {
                    {nameof(id), id},
                };

                _managementLogs.WriteLog(ManagementExceptions.GetMessageSystemError(e),
                    "Error al conectar a BD",
                    data);

                throw e.InnerException ?? e;
            }
        }

        private void GuardarLog(string username, string rol, string evento, string tablaDB, string Operación, string Detalle_registro)
        {
            //TODO: Guardar traza de Acceso en auditoria
            var log = new LogsAuditorium
            {
                IdUsuario = username,
                IdOperacion = Operación!,
                TpUsuario = rol,
                FechaConexion = DateTime.Now,
                FechaIngreso = DateTime.Now,
                IpDispositivo = "localhost",
                BdTabla = tablaDB!,
                TpEvento = evento,
                Modulo = "Subscripción Alertas",
                DetalleRegistro = Detalle_registro
            };
            _context.LogsAuditoria.Add(log);
            _context.SaveChanges();

        }

        // Función auxiliar para obtener la dirección IP del usuario
        private string GetUserIPAddress()
        {
            // 1. Desde el Request (puede ser null si está detrás de un proxy)
            string? ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            // 2. Desde los headers (si estás detrás de un proxy)
            if (string.IsNullOrEmpty(ipAddress) && Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = Request.Headers["X-Forwarded-For"];
            }

            // 3. Si usas Kestrel y no estás detrás de un proxy
            if (string.IsNullOrEmpty(ipAddress) && Request.HttpContext.Connection.LocalIpAddress != null)
            {
                ipAddress = Request.HttpContext.Connection.LocalIpAddress.ToString();
            }

            return ipAddress ?? "Unknown";
        }
    }
}
