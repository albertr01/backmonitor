using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MstOcupacion
{
    public int Id { get; set; }

    public string? Codigo { get; set; }

    public string ProfesionOcupacion { get; set; } = null!;

    public string? NivelRiesgo { get; set; }
}
