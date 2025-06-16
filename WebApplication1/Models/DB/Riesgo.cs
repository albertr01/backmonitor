using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class Riesgo
{
    public int Id { get; set; }

    public int? FkPeriodo { get; set; }

    public string? Amenaza { get; set; }

    public string? Vulnerabilidad { get; set; }

    public string? Consecuencia { get; set; }

    public int? FkFactorRiesgo { get; set; }

    public string? Causa { get; set; }

    public int? FkProbabilidad { get; set; }

    public string? ProbabilidadValor { get; set; }

    public int? FkImpacto { get; set; }

    public string? ImpactoValor { get; set; }

    public string? RiesgoInherente { get; set; }

    public string? Severidad { get; set; }

    public string? Descripcion { get; set; }

    public int? FkTipoRiesgo { get; set; }

    public int? FkAutomatizacion { get; set; }

    public int? FkFrecuencia { get; set; }

    public string? ValoresControl { get; set; }

    public string? RiesgoResidualValor { get; set; }

    public string? SeveridadValor { get; set; }

    public string? Accion { get; set; }

    public string? Responsable { get; set; }

    public int? FkTiempoEjecucion { get; set; }

    public int? FkTratamientoRiesgo { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int? FkSubFactorRiesgo { get; set; }

    public int? FktipoRiesgoR { get; set; }

    public string? Nombre { get; set; }

    public virtual MstParametrosRiesgo? FkAutomatizacionNavigation { get; set; }

    public virtual MstParametrosRiesgo? FkFactorRiesgoNavigation { get; set; }

    public virtual MstParametrosRiesgo? FkFrecuenciaNavigation { get; set; }

    public virtual MstParametrosRiesgo? FkImpactoNavigation { get; set; }

    public virtual Periodo? FkPeriodoNavigation { get; set; }

    public virtual MstParametrosRiesgo? FkProbabilidadNavigation { get; set; }

    public virtual MstParametrosRiesgo? FkSubFactorRiesgoNavigation { get; set; }

    public virtual MstParametrosRiesgo? FkTipoRiesgoNavigation { get; set; }

    public virtual MstParametrosRiesgo? FktipoRiesgoRNavigation { get; set; }
}
