using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClientesEconomiaRelacionDependencium
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string? EmpleoEmpresa { get; set; }

    public decimal? EmpleoSalario { get; set; }

    public DateOnly? EmpleoFechaIngreso { get; set; }

    public string? EmpleoContactoTelefono { get; set; }

    public string? EmpleoContactoCorreo { get; set; }
}
