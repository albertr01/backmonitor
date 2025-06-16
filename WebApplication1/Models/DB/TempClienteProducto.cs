using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class TempClienteProducto
{
    public int? IdCliente { get; set; }

    public string? TipoProducto { get; set; }

    public string? Codigo { get; set; }

    public string? NumeroCuenta { get; set; }

    public decimal? Umbral { get; set; }

    public decimal? TransaccionesPromedio { get; set; }

    public decimal? MontoPromedio { get; set; }

    public DateTime? UltimoMov { get; set; }

    public DateOnly? FechaApertura { get; set; }
}
