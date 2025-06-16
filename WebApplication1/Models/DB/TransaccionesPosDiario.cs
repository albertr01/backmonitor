using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class TransaccionesPosDiario
{
    public int Id { get; set; }

    public string IdTransaccionPos { get; set; } = null!;

    public string IdCliente { get; set; } = null!;

    public DateTime FechaHoraTransaccion { get; set; }

    public decimal MontoTransaccion { get; set; }

    public string? CodigoComercio { get; set; }

    public string? NombreComercio { get; set; }

    public string? CategoriaComercio { get; set; }

    public decimal? LatitudComercio { get; set; }

    public decimal? LongitudComercio { get; set; }

    public string? DireccionComercio { get; set; }
}
