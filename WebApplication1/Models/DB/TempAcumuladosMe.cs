using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class TempAcumuladosMe
{
    public int? IdCliente { get; set; }

    public string? NumeroCuenta { get; set; }

    public string? Mes { get; set; }

    public int? TransaccionesNoFrecuentes { get; set; }

    public int? Cantidad { get; set; }

    public decimal? Monto { get; set; }
}
