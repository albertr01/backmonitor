using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MstNacionalidad
{
    public int Id { get; set; }

    public int? IdPais { get; set; }

    public string? Codigo { get; set; }

    public string Nacionalidad { get; set; } = null!;

    public string? NivelRiesgo { get; set; }
}
