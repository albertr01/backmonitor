using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MstTablasParametro
{
    public int Id { get; set; }

    public string NombreParametro { get; set; } = null!;

    public int? TipoTabla { get; set; }

    public string TablaParametro { get; set; } = null!;

    public bool EstatusParametro { get; set; }
}
