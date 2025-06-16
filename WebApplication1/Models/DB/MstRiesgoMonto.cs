using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MstRiesgoMonto
{
    public int Id { get; set; }

    public string? Codigo { get; set; }

    public string TipoMoneda { get; set; } = null!;

    public decimal? Desde { get; set; }

    public decimal? Hasta { get; set; }

    public string? NivelRiesgo { get; set; }
}
