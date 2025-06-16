using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class Periodo
{
    public int Id { get; set; }

    public string? Periodo1 { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaCierre { get; set; }

    public string? Estatus { get; set; }

    public string? TipoPeriodo { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Riesgo> Riesgos { get; set; } = new List<Riesgo>();
}
