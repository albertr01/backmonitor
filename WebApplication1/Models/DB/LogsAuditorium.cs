using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class LogsAuditorium
{
    public int Id { get; set; }

    public string IdUsuario { get; set; } = null!;

    public string? IdOperacion { get; set; }

    public string? TpUsuario { get; set; }

    public DateTime? FechaConexion { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public DateTime? FechaDesconexion { get; set; }

    public string? IpDispositivo { get; set; }

    public string? BdTabla { get; set; }

    public string? TpEvento { get; set; }

    public string? Modulo { get; set; }

    public string? Menu { get; set; }

    public string? Submenu { get; set; }

    public string? Opciones { get; set; }

    public int? IntentosExitosos { get; set; }

    public int? IntentosFallidos { get; set; }

    public string? DetalleRegistro { get; set; }
}
