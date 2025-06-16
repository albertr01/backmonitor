using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MstParametrosRiesgo
{
    public int Id { get; set; }

    public string TipoParametroRiesgo { get; set; } = null!;

    public int? ValorIn { get; set; }

    public int? ValorFn { get; set; }

    public string? Color { get; set; }

    public int IdTblParametro { get; set; }

    public int? IdMtsParametrosRiegos { get; set; }

    public virtual MstParametrosRiesgo? IdMtsParametrosRiegosNavigation { get; set; }

    public virtual ICollection<MstParametrosRiesgo> InverseIdMtsParametrosRiegosNavigation { get; set; } = new List<MstParametrosRiesgo>();

    public virtual ICollection<Riesgo> RiesgoFkAutomatizacionNavigations { get; set; } = new List<Riesgo>();

    public virtual ICollection<Riesgo> RiesgoFkFactorRiesgoNavigations { get; set; } = new List<Riesgo>();

    public virtual ICollection<Riesgo> RiesgoFkFrecuenciaNavigations { get; set; } = new List<Riesgo>();

    public virtual ICollection<Riesgo> RiesgoFkImpactoNavigations { get; set; } = new List<Riesgo>();

    public virtual ICollection<Riesgo> RiesgoFkProbabilidadNavigations { get; set; } = new List<Riesgo>();

    public virtual ICollection<Riesgo> RiesgoFkSubFactorRiesgoNavigations { get; set; } = new List<Riesgo>();

    public virtual ICollection<Riesgo> RiesgoFkTipoRiesgoNavigations { get; set; } = new List<Riesgo>();

    public virtual ICollection<Riesgo> RiesgoFktipoRiesgoRNavigations { get; set; } = new List<Riesgo>();
}
