using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class MstProductosServicio
{
    public int Id { get; set; }

    public string? Codigo { get; set; }

    public string Denominacion { get; set; } = null!;

    public DateTime? FechaAutorizacion { get; set; }

    public string ProductosServicios { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public string? NivelRiesgo { get; set; }
}
