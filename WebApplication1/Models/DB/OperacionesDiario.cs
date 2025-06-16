using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class OperacionesDiario
{
    public int Id { get; set; }

    public string IdTransaccion { get; set; } = null!;

    public string IdCliente { get; set; } = null!;

    public DateTime FechaHoraTransaccion { get; set; }

    public decimal MontoTransaccion { get; set; }

    public string MonedaTransaccion { get; set; } = null!;

    public string? CodigoOficina { get; set; }

    public string? NombreOficina { get; set; }

    public decimal? LatitudOficina { get; set; }

    public decimal? LongitudOficina { get; set; }

    public string? DireccionOficina { get; set; }

    public string? TipoOperacion { get; set; }
}
