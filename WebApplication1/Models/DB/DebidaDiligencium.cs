using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class DebidaDiligencium
{
    public int Id { get; set; }

    public string? IdentificacionCliente { get; set; }

    public bool? DebidaDiligencia { get; set; }

    public bool? DebidaDiligenciaVencida { get; set; }

    public DateTime? FechaDebidaDiligencia { get; set; }

    public string? Observaciones { get; set; }

    public string? TipoDebidaDiligencia { get; set; }
}
