using System;
using System.Collections.Generic;

namespace WebApplication1.Models.DB;

public partial class ClienteProducto
{
    public int Id { get; set; }

    public string IdCliente { get; set; } = null!;

    public string ProductoNombre { get; set; } = null!;

    public string ProductoMoneda { get; set; } = null!;

    public string ProductoUso { get; set; } = null!;

    public int ProductoTransaccionesPromedio { get; set; }

    public decimal ProductoMontoPromedio { get; set; }

    public string ProductoOrigenFondos { get; set; } = null!;

    public string ProductoDestinoFondos { get; set; } = null!;

    public decimal MontoUmbral { get; set; }

    public DateOnly FechaApertura { get; set; }

    public string OficinaApertura { get; set; } = null!;

    public string NumeroCuenta { get; set; } = null!;
}
