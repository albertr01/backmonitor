using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MstCanalesAtencion
{
    public int Id { get; set; }

    public string? Codigo { get; set; }

    public string CanalesAtencion { get; set; } = null!;

    public string? NivelRiesgo { get; set; }
}
