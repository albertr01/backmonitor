using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClienteEconomiaOtrosIngreso
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string OtroIngresoFuente { get; set; } = null!;

    public decimal? OtroIngresoMonto { get; set; }

    public string? OtroIngresoFrecuencia { get; set; }
}
