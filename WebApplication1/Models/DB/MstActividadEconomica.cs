using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MstActividadEconomica
{
    public int Id { get; set; }

    public string? Codigo { get; set; }

    public bool Apnfd { get; set; }

    public string Sector { get; set; } = null!;

    public string ActividadEconomicaEspecifica { get; set; } = null!;

    public string? NivelRiesgo { get; set; }
}
