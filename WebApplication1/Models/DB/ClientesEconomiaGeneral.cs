using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClientesEconomiaGeneral
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string? ActividadEconomicaGeneral { get; set; }

    public string? ActividadEconomicaEspecifica { get; set; }

    public string? FuenteIngresos { get; set; }
}
