using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class SubcripcionAlertum
{
    public int Id { get; set; }

    public int? FkAlerta { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? MetodoDistribucion { get; set; }

    public int? BandejaEntrada { get; set; }

    public string? Comentario { get; set; }

    public bool? Estado { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public string? Usuario { get; set; }

    public int? IdCliente { get; set; }

    public bool? NotificacionCorreo { get; set; }

    public string? Correo { get; set; }

    public virtual Alertum? FkAlertaNavigation { get; set; }
}
