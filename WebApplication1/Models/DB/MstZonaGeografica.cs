using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MstZonaGeografica
{
    public int Id { get; set; }

    public string? Codigo { get; set; }

    public string Estado { get; set; } = null!;

    public string Capital { get; set; } = null!;

    public string? RiesgoInherente { get; set; }

    public string? NivelRiesgo { get; set; }
}
